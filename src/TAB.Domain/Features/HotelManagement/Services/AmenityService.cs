using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Domain.Features.HotelManagement.Services;

public class AmenityService : IAmenityService
{
    private readonly IHotelRepository _hotelRepository;

    public AmenityService(IHotelRepository hotelRepository) => _hotelRepository = hotelRepository;

    public Task<Result> CheckAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        AmenityType amenityType,
        CancellationToken cancellationToken = default
    ) =>
        amenityType switch
        {
            AmenityType.Hotel
                => CheckHotelAmenityTypeAndUserOwnerShipAsync(userId, typeId, cancellationToken),
            AmenityType.Room => throw new NotImplementedException(),
            _
                => throw new ArgumentOutOfRangeException(
                    nameof(amenityType),
                    amenityType,
                    "This type not supported now!"
                )
        };

    private async Task<Result> CheckHotelAmenityTypeAndUserOwnerShipAsync(
        int userId,
        int typeId,
        CancellationToken cancellationToken = default
    )
    {
        var maybeHotel = await _hotelRepository.GetByIdAsync(typeId, cancellationToken);

        if (maybeHotel.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        return !IsHotelOwner(userId, maybeHotel.Value)
            ? DomainErrors.General.Unauthorized
            : Result.Success();
    }

    private static bool IsHotelOwner(int userId, Hotel hotel) => hotel.OwnerId == userId;
}
