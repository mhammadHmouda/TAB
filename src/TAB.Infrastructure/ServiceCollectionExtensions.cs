using Microsoft.Extensions.DependencyInjection;
using TAB.Domain.Core.Interfaces;
using TAB.Infrastructure.Common;

namespace TAB.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
