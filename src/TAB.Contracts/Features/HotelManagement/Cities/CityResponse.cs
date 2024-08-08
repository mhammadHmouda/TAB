namespace TAB.Contracts.Features.HotelManagement.Cities;

public record CityResponse(
    int Id,
    string Name,
    string Country,
    string PostOffice,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc
);
