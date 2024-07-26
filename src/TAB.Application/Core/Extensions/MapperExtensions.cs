using Microsoft.Extensions.DependencyInjection;

namespace TAB.Application.Core.Extensions;

public static class MapperExtensions
{
    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        return services;
    }
}
