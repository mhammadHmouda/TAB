using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Contracts.Features.HotelManagement;

public record CreateHotelRequest(
    string Name,
    string Description,
    double Latitude,
    double Longitude,
    int CityId,
    int OwnerId,
    HotelType Type
);
