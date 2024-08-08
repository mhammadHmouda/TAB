using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Contracts.Features.HotelManagement.Rooms;

public record UpdateRoomRequest(
    int Id,
    int Number,
    decimal Price,
    string Currency,
    RoomType Type,
    int CapacityOfAdults,
    int CapacityOfChildren
);
