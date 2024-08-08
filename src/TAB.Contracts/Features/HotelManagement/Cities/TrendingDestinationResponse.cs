namespace TAB.Contracts.Features.HotelManagement.Cities;

public record TrendingDestinationResponse(
    string CityName,
    string CountryName,
    string? ThumbnailUrl
);
