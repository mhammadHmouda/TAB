using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.HotelManagement.Hotels;

public record FeaturedDealsResponse(
    int HotelId,
    string HotelName,
    string? ThumbnailUrl,
    Location Location,
    Money? PricePerNight,
    int? DiscountedPrice,
    IReadOnlyCollection<RoomDeal> RoomDeals
);

public record RoomDeal(
    int Id,
    int Number,
    string Type,
    Money PricePerNight,
    Money DiscountedPrice,
    IReadOnlyCollection<DiscountResponse> Discounts
);
