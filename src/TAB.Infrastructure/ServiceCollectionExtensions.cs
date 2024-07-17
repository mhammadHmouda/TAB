using Microsoft.Extensions.DependencyInjection;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Domain.Core.Interfaces;
using TAB.Infrastructure.Common;
using TAB.Infrastructure.Cryptography;

namespace TAB.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();

        return services;
    }
}
