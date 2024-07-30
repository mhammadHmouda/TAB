using TAB.Contracts.Features.Shared.Email;

namespace TAB.Application.Core.Interfaces.Notifications;

public interface IEmailNotificationService
{
    Task SendWelcomeEmail(WelcomeEmail welcomeEmail);
    Task SendSuccessBookingEmail(BookingSuccessEmail bookingSuccessEmail);
}
