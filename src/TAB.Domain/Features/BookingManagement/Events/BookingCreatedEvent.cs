using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Domain.Features.BookingManagement.Events;

public record BookingCreatedEvent(
    int UserId,
    int HotelId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    Money TotalPrice
) : IDomainEvent;
