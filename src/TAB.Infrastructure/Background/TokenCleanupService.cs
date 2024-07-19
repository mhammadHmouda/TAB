using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Infrastructure.Background;

public class TokenCleanupService : BackgroundService
{
    private readonly ILogger<TokenCleanupService> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TokenCleanupService(
        ILogger<TokenCleanupService> logger,
        IServiceProvider serviceProvider
    )
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var now = DateTime.UtcNow;
            var nextRun = now.AddDays(1).AddHours(1);
            var delay = nextRun - now;

            _logger.LogInformation(
                $"Token cleanup service will run in {delay.TotalMinutes} minutes."
            );

            await Task.Delay(delay, cancellationToken);

            if (!cancellationToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();
                var tokenRepository = scope.ServiceProvider.GetRequiredService<ITokenRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                tokenRepository.RemoveExpiredTokens();
                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
