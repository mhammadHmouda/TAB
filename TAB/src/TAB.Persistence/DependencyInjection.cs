using Microsoft.Extensions.DependencyInjection;

namespace TAB.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services) => services;
}
