using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.BookingManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public class BookingCancelledEventHandler : IDomainEventHandler<BookingCancelledEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IBookingRepository _bookingRepository;

    public BookingCancelledEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
    }

    public async Task Handle(BookingCancelledEvent domainEvent, CancellationToken cancellationToken)
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

        var result = booking.Room.UpdateAvailability(true);

        if (result.IsFailure)
        {
            return;
        }

        await _emailNotificationService.SendBookingCancelledEmail(
            new BookingCancelledEmail(
                $"{booking.User.FirstName} {booking.User.LastName}",
                booking.User.Email,
                booking.Hotel.Name
            )
        );
    }
}
