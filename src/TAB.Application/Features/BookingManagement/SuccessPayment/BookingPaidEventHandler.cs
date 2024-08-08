using TAB.Application.Core.Interfaces.Notifications;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.SuccessPayment;

public class BookingPaidEventHandler : IDomainEventHandler<BookingPaidEvent>
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    public BookingPaidEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingPaidEvent notification, CancellationToken cancellationToken)
    {
        var maybeBooking = await _bookingRepository.GetAsync(
            new BookingWithAllInfoSpecification(notification.BookingId),
            cancellationToken
        );

        if (maybeBooking.HasNoValue)
            return;

        var booking = maybeBooking.Value;

        var email = new BookingPaidEmail(
            booking.Id,
            booking.User.Email,
            $"{booking.User.FirstName} {booking.User.LastName}",
            booking.Hotel.Name,
            booking.TotalPrice.Amount,
            booking.TotalPrice.Currency
        );

        await _emailNotificationService.SendBookingPayedEmail(email);
    }
}
