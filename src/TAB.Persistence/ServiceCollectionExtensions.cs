using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Persistence.Infrastructure;
using TAB.Persistence.Repositories.HotelManagement;
using TAB.Persistence.Repositories.UserManagement;

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

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ITokenRepository, TokenRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IHotelRepository, HotelRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();

        return services;
    }
}
