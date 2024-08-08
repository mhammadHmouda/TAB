namespace TAB.Contracts.Features.HotelManagement.Hotels;

public record UpdateHotelRequest(
    int Id,
    string Name,
    string Description,
    double Latitude,
    double Longitude
);
