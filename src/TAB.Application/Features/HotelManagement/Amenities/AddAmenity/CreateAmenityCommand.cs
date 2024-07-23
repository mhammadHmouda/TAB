using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Application.Features.HotelManagement.Amenities.AddAmenity;

public record CreateAmenityCommand(string Name, string Description, AmenityType Type, int TypeId)
    : ICommand<Result<AmenityResponse>>;
