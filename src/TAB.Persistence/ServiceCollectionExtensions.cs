using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TAB.Application.Core.Interfaces;
using TAB.Domain.Features.Todos;
using TAB.Persistence.Infrastructure;
using TAB.Persistence.Repositories;

namespace TAB.Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        var connectionString = configuration.GetConnectionString(ConnectionString.SectionName)!;

        services.AddSingleton(new ConnectionString(connectionString));

        services.AddDbContext<TabDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IDbContext>(serviceProvider =>
            serviceProvider.GetService<TabDbContext>()!
        );

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<ITodoRepository, TodoRepository>();

        return services;
    }
}
