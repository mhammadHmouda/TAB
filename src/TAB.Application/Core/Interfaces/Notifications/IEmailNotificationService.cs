using TAB.Contracts.Features.UserManagement;

namespace TAB.Application.Core.Interfaces.Notifications;

public interface IEmailNotificationService
{
    Task SendWelcomeEmail(WelcomeEmail welcomeEmail);
}
