namespace TAB.Contracts.Features.HotelManagement;

public record CityResponse(
    int Id,
    string Name,
    string Country,
    string PostOffice,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
