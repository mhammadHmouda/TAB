using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.HotelManagement.Rooms;

public class RoomSearchResponse
{
    public int Id { get; init; }
    public int Number { get; init; }
    public string Description { get; init; }
    public Money Price { get; init; }
    public decimal DiscountedPrice { get; init; }
    public RoomType Type { get; init; }
    public bool IsAvailable { get; init; }
    public int AdultsCapacity { get; init; }
    public int ChildrenCapacity { get; init; }
    public DiscountResponse[] Discounts { get; init; }
    public ImageResponse[] Images { get; set; }
    public AmenityResponse[] Amenities { get; set; }
}
