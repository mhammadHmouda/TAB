using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Contracts.Features.HotelManagement.Rooms;

public record CreateRoomRequest(
    int HotelId,
    int Number,
    string Description,
    decimal Price,
    string Currency,
    RoomType Type,
    int CapacityOfAdults = 2,
    int CapacityOfChildren = 0
);
