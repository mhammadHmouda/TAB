using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Contracts.Features.HotelManagement.Hotels;

public class HotelSearchResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }
    public Location Location { get; init; }
    public string City { get; init; }
    public int StarRating { get; init; }
    public int NumberOfAvailableRooms { get; init; }
    public string Type { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime? UpdatedAtUtc { get; init; }
    public RoomResponse[] Rooms { get; init; }
    public ImageResponse[] Images { get; set; }
    public AmenityResponse[] Amenities { get; set; }
}
