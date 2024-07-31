namespace TAB.Contracts.Features.BookingManagement;

public record BookingRoomRequest(DateTime CheckInDate, DateTime CheckOutDate, int RoomId);
