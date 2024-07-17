using Microsoft.Extensions.Configuration;
using TAB.Application.Core.Interfaces.Email;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.UserManagement;

namespace TAB.Infrastructure.Notifications;

public class EmailNotificationService : IEmailNotificationService
{
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
    private string BaseUrl => $"{_configuration["Host:Url"]!}/api/v1/";

    public EmailNotificationService(IEmailService emailService, IConfiguration configuration)
    {
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task SendWelcomeEmail(WelcomeEmail welcomeEmail)
    {
        var body = $"""
            Welcome to Tab, {welcomeEmail.Name}!

            Please click the following link to verify your email address: {BaseUrl}auth/activate?token={welcomeEmail.Token}

            Thank you for using Tab!
            """;

        var mailRequest = new MailRequest(welcomeEmail.EmailTo, "Welcome to Tap", body);
        await _emailService.SendEmailAsync(mailRequest);
    }
}
