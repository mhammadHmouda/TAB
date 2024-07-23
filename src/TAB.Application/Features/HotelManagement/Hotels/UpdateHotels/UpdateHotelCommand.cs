using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Hotels.UpdateHotels;

public record UpdateHotelCommand(
    int Id,
    string Name,
    string Description,
    double Latitude,
    double Longitude
) : ICommand<Result>;
