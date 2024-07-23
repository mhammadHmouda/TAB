using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Domain.Features.HotelManagement.Services;

public interface IAmenityService
{
    Task<Result> CheckAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        AmenityType amenityType,
        CancellationToken cancellationToken = default
    );
}
