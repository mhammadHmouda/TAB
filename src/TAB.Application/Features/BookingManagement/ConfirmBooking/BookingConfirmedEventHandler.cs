using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.ConfirmBooking;

public class BookingConfirmedEventHandler : IDomainEventHandler<BookingConfirmedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IBookingRepository _bookingRepository;
    private readonly IHotelRepository _hotelRepository;
    private readonly IUserRepository _userRepository;

    public BookingConfirmedEventHandler(
        IEmailNotificationService emailNotificationService,
        IBookingRepository bookingRepository,
        IHotelRepository hotelRepository,
        IUserRepository userRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _bookingRepository = bookingRepository;
        _hotelRepository = hotelRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(BookingConfirmedEvent domainEvent, CancellationToken cancellationToken)
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

        var hotel = hotelMaybe.Value;

        var email = new BookingConfirmedEmail(
            booking.Id,
            $"{user.FirstName} {user.LastName}",
            user.Email,
            hotel.Name,
            booking.CheckInDate.ToLongDateString(),
            booking.CheckOutDate.ToLongDateString(),
            booking.TotalPrice.Amount,
            booking.TotalPrice.Currency
        );

        await _emailNotificationService.SendBookingConfirmedEmail(email);
    }
}
