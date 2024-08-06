using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Email;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.HotelManagement.Services;
using TAB.Infrastructure.Authentication;
using TAB.Infrastructure.Authentication.Options;
using TAB.Infrastructure.Background;
using TAB.Infrastructure.Common;
using TAB.Infrastructure.Common.Options;
using TAB.Infrastructure.Cryptography;
using TAB.Infrastructure.Emails;
using TAB.Infrastructure.Emails.Options;
using TAB.Infrastructure.Notifications;
using TAB.Infrastructure.Payment;
using TAB.Infrastructure.Payment.Options;

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
        services.AddScoped<IJwtProvider, JwtProvider>();
        services.AddScoped<ITokenValidationService, TokenValidationService>();
        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IGeneratorService, GeneratorService>();
        services.AddScoped<IUploadFileService, AzureBlobService>();
        services.AddScoped<IAmenityService, AmenityService>();

        services.AddHostedService<TokenCleanupService>();

        services.AddAuthentication(configuration);
        services.AddAzureBlob();

        services.AddPaymentService(configuration);

        return services;
    }

    private static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionKey));
        services.ConfigureOptions<ConfigureJwtBearerOptions>();

        services.AddAuthorization();

        return services;
    }

    public static IServiceCollection AddAzureBlob(this IServiceCollection services)
    {
        services.ConfigureOptions<ConfigureAzureBlobOptions>();

        services.AddSingleton(provider =>
        {
            var options = provider.GetRequiredService<IOptions<AzureBlobOptions>>().Value;
            return new BlobServiceClient(options.ConnectionString);
        });

        return services;
    }

    public static IServiceCollection AddPaymentService(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.Configure<StripeOptions>(configuration.GetSection(StripeOptions.SectionName));
        services.Configure<PayPalOptions>(configuration.GetSection(PayPalOptions.SectionName));

        var stripeOptions = services
            .BuildServiceProvider()
            .GetRequiredService<IOptions<StripeOptions>>()
            .Value;

        StripeConfiguration.ApiKey = stripeOptions.SecretKey;

        services.AddScoped<SessionService>();
        services.AddScoped<IPaymentService, PaymentService>();

        return services;
    }
}
