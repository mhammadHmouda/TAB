using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.BookingManagement;

public record BookingResponse(
    int Id,
    int HotelId,
    int RoomId,
    int UserId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    Money TotalPrice,
    string Status
);
