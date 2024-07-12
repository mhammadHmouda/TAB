﻿using Microsoft.Extensions.DependencyInjection;
using TAB.Domain.Core.Abstractions;
using TAB.Infrastructure.Common;

namespace TAB.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}
