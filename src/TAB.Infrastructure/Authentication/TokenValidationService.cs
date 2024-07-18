using TAB.Application.Core.Interfaces.Common;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Infrastructure.Authentication;

public class TokenValidationService : ITokenValidationService
{
    private readonly IDateTimeProvider _dateTime;
    private readonly ITokenRepository _tokenRepository;

    public TokenValidationService(IDateTimeProvider dateTime, ITokenRepository tokenRepository)
    {
        _dateTime = dateTime;
        _tokenRepository = tokenRepository;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var tokenEntity = await _tokenRepository.GetByAsync(
            t => t.Value == token,
            CancellationToken.None
        );

        return tokenEntity is { HasNoValue: false, Value.IsRevoked: false }
            && tokenEntity.Value.ExpiresAt >= _dateTime.UtcNow;
    }
}
