using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.HotelManagement.Rooms;

public record RoomResponse(
    int Id,
    int Number,
    string Description,
    Money Price,
    decimal DiscountedPrice,
    RoomType Type,
    bool IsAvailable,
    int CapacityOfAdults,
    int CapacityOfChildren
);
