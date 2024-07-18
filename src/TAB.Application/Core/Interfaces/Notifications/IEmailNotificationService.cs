using TAB.Contracts.Features.UserManagement.Email;

namespace TAB.Application.Core.Interfaces.Notifications;

public interface IEmailNotificationService
{
    Task SendWelcomeEmail(WelcomeEmail welcomeEmail);
}
