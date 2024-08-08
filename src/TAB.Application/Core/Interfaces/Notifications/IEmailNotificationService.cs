using TAB.Contracts.Features.Shared.Email;

namespace TAB.Application.Core.Interfaces.Notifications;

public interface IEmailNotificationService
{
    Task SendWelcomeEmail(WelcomeEmail welcomeEmail);
    Task SendSuccessBookingEmail(BookingSuccessEmail bookingSuccessEmail);
    Task SendBookingConfirmedEmail(BookingConfirmedEmail bookingConfirmedEmail);
    Task SendBookingCancelledEmail(BookingCancelledEmail bookingCanceledEmail);
    Task SendBookingPayedEmail(BookingPaidEmail bookingPaidEmail);
}
