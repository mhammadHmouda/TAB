using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Email;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Domain.Core.Interfaces;
using TAB.Infrastructure.Common;
using TAB.Infrastructure.Cryptography;
using TAB.Infrastructure.Emails;
using TAB.Infrastructure.Emails.Options;
using TAB.Infrastructure.Notifications;

namespace TAB.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<MailOptions>(configuration.GetSection(MailOptions.SectionName).Bind);

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();
        services.AddScoped<ITokenGenerator, GuidTokenGenerator>();

        return services;
    }
}
