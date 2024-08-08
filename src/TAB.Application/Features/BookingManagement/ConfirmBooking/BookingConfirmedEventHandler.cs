using TAB.Application.Core.Interfaces.Notifications;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.ConfirmBooking;

public class BookingConfirmedEventHandler : IDomainEventHandler<BookingConfirmedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IBookingRepository _bookingRepository;

    public BookingConfirmedEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingConfirmedEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookingMaybe = await _bookingRepository.GetAsync(
            new BookingWithAllInfoSpecification(domainEvent.BookingId),
            cancellationToken
        );

        if (bookingMaybe.HasNoValue)
        {
            return;
        }

        var booking = bookingMaybe.Value;

        var user = booking.User;

        var email = new BookingConfirmedEmail(
            booking.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email,
            booking.Hotel.Name,
            booking.CheckInDate.ToLongDateString(),
            booking.CheckOutDate.ToLongDateString(),
            booking.TotalPrice.Amount,
            booking.TotalPrice.Currency
        );

        await _emailNotificationService.SendBookingConfirmedEmail(email);
    }
}
