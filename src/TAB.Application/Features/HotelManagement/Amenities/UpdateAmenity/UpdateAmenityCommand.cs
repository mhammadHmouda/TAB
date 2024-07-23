using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Amenities.UpdateAmenity;

public record UpdateAmenityCommand(int Id, string Name, string Description)
    : ICommand<Result<AmenityResponse>>;
