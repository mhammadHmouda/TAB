namespace TAB.Contracts.Features.HotelManagement.Hotels;

public record HotelResponse(
    int Id,
    string Name,
    string Description,
    double Latitude,
    double Longitude,
    string City,
    string Country,
    string Type,
    string Owner
);
