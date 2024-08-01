using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.HotelManagement.Rooms;

public record RoomSearchResponse(
    int Id,
    int Number,
    string Description,
    Money Price,
    decimal DiscountedPrice,
    RoomType Type,
    bool IsAvailable,
    int AdultsCapacity,
    int ChildrenCapacity,
    DiscountResponse[] Discounts,
    ImageResponse[] Images,
    AmenityResponse[] Amenities
);
