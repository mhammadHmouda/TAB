using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TAB.Application.Core.Behaviours;

namespace TAB.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(m =>
        {
            m.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            m.AddOpenBehavior(typeof(TransactionBehaviour<,>));
            m.AddOpenBehavior(typeof(ValidationBehaviour<,>));
        });

        services.AddValidatorsFromAssembly(assembly);

        return services;
    }
}
