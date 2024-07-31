using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.CancelBooking;

public class BookingCancelledEventHandler : IDomainEventHandler<BookingCancelledEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;
    private readonly IRoomRepository _roomRepository;

    public BookingCancelledEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository,
        IHotelRepository hotelRepository,
        IUserRepository userRepository,
        IRoomRepository roomRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
        _roomRepository = roomRepository;
    }

    public async Task Handle(BookingCancelledEvent domainEvent, CancellationToken cancellationToken)
    {
        var bookingMaybe = await _bookingRepository.GetByIdAsync(
            domainEvent.BookingId,
            cancellationToken
        );

        if (bookingMaybe.HasNoValue)
        {
            return;
        }

        var booking = bookingMaybe.Value;

        var userMaybe = await _userRepository.GetByIdAsync(booking.UserId, cancellationToken);

        if (userMaybe.HasNoValue)
        {
            return;
        }

        var user = userMaybe.Value;

        var hotelMaybe = await _hotelRepository.GetByIdAsync(booking.HotelId, cancellationToken);

        if (hotelMaybe.HasNoValue)
        {
            return;
        }

        var roomMaybe = await _roomRepository.GetByIdAsync(booking.RoomId, cancellationToken);

        if (roomMaybe.HasNoValue)
        {
            return;
        }

        var result = roomMaybe.Value.UpdateAvailability(true);

        if (result.IsFailure)
        {
            return;
        }

        await _emailNotificationService.SendBookingCancelledEmail(
            new BookingCancelledEmail(
                $"{user.FirstName} {user.LastName}",
                user.Email,
                hotelMaybe.Value.Name
            )
        );
    }
}
