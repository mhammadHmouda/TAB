using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.BookingManagement.Events;

public record BookingCreatedEvent(
    int UserId,
    int HotelId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice,
    string Currency
) : IDomainEvent;
