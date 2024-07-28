namespace TAB.Contracts.Features.BookingManagement;

public record BookingResponse(
    int Id,
    int HotelId,
    int RoomId,
    int UserId,
    DateTime CheckInDate,
    DateTime CheckOutDate,
    decimal TotalPrice
);
