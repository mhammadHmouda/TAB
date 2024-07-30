using MediatR;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.BookingManagement.AddBooking;

public class BookingCreatedEventHandler : INotificationHandler<BookingCreatedEvent>
{
    private readonly IEmailNotificationService _emailNotificationService;
    private readonly IUserRepository _userRepository;
    private readonly IHotelRepository _hotelRepository;

    public BookingCreatedEventHandler(
        IEmailNotificationService emailNotificationService,
        IUserRepository userRepository,
        IHotelRepository hotelRepository
    )
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
        _hotelRepository = hotelRepository;
    }

    public async Task Handle(BookingCreatedEvent notification, CancellationToken cancellationToken)
    {
        var maybeUser = await _userRepository.GetByIdAsync(notification.UserId, cancellationToken);

        if (maybeUser.HasNoValue)
        {
            return;
        }

        var maybeHotel = await _hotelRepository.GetByIdAsync(
            notification.HotelId,
            cancellationToken
        );

        if (maybeHotel.HasNoValue)
        {
            return;
        }

        var user = maybeUser.Value;
        var hotel = maybeHotel.Value;

        var bookingSuccessEmail = new BookingSuccessEmail(
            $"{user.FirstName} {user.LastName}",
            user.Email,
            hotel.Name,
            notification.CheckInDate.ToLongDateString(),
            notification.CheckOutDate.ToLongDateString(),
            notification.TotalPrice,
            notification.Currency
        );

        await _emailNotificationService.SendSuccessBookingEmail(bookingSuccessEmail);
    }
}
