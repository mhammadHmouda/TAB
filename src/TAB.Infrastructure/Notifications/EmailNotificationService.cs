using Microsoft.Extensions.Configuration;
using TAB.Application.Core.Interfaces.Email;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.Shared.Email;

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

    public async Task SendSuccessBookingEmail(BookingSuccessEmail bookingSuccessEmail)
    {
        var body = $"""
            Dear {bookingSuccessEmail.Name},

            Your booking at {bookingSuccessEmail.HotelName} has been successfully created.

            Please find the booking details below:

            Check in date: {bookingSuccessEmail.CheckInDate}
            Check out date: {bookingSuccessEmail.CheckOutDate}
            Total price: {bookingSuccessEmail.TotalPrice.Amount} {bookingSuccessEmail
                .TotalPrice
                .Currency}

            Your booking now in pending status. Please wait for the hotel to confirm your booking.

            Thank you for using Tab!
            """;

        var mailRequest = new MailRequest(bookingSuccessEmail.EmailTo, "Booking Success", body);

        await _emailService.SendEmailAsync(mailRequest);
    }

    public Task SendBookingConfirmedEmail(BookingConfirmedEmail bookingConfirmedEmail)
    {
        var body = $"""
            Dear {bookingConfirmedEmail.Name},

            Your booking at {bookingConfirmedEmail.HotelName} has been confirmed.

            Please find the booking details below:
            Check in date: {bookingConfirmedEmail.CheckInDate}.
            Check out date: {bookingConfirmedEmail.CheckOutDate}.
            Total price: {bookingConfirmedEmail.TotalPrice} {bookingConfirmedEmail.Currency}.

            to checkout, please click the following link: {BaseUrl}bookings/{bookingConfirmedEmail.BookingId}/checkout

            Thank you for using Tab!
            """;

        var mailRequest = new MailRequest(bookingConfirmedEmail.EmailTo, "Booking Confirmed", body);

        return _emailService.SendEmailAsync(mailRequest);
    }

    public Task SendBookingCancelledEmail(BookingCancelledEmail bookingCanceledEmail)
    {
        var body = $"""
            Dear {bookingCanceledEmail.Name},

            your booking at {bookingCanceledEmail.HotelName} has been cancelled.

            Thank you for using Tab!
            """;

        var mailRequest = new MailRequest(bookingCanceledEmail.EmailTo, "Booking Cancelled", body);

        return _emailService.SendEmailAsync(mailRequest);
    }

    public Task SendBookingPayedEmail(BookingPaidEmail bookingPayedEmail)
    {
        var body = $"""
            Dear {bookingPayedEmail.Name},

            Thank you for choosing Tab!
            your booking {bookingPayedEmail.BookingId} in {bookingPayedEmail.HotelName} has been paid.

            Total price: {bookingPayedEmail.TotalPrice} {bookingPayedEmail.Currency}

            Thank you for choosing Tab! , Hope to see you again.
            
            """;

        var mailRequest = new MailRequest(bookingPayedEmail.EmailTo, "Booking Paid", body);

        return _emailService.SendEmailAsync(mailRequest);
    }
}
