using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Application.Features.HotelManagement.Hotels.AddHotels;

public record CreateHotelCommand(
    string Name,
    string Description,
    double Latitude,
    double Longitude,
    int CityId,
    int OwnerId,
    HotelType Type
) : ICommand<Result<HotelResponse>>;
