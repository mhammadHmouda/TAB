using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Domain.Features.HotelManagement.Services;

public class AmenityService : IAmenityService
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IRoomRepository _roomRepository;

    public AmenityService(IHotelRepository hotelRepository, IRoomRepository roomRepository)
    {
        _hotelRepository = hotelRepository;
        _roomRepository = roomRepository;
    }

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
            AmenityType.Room => CheckRoomAmenityType(typeId, cancellationToken),
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

        return userId != maybeHotel.Value.OwnerId
            ? DomainErrors.General.Unauthorized
            : Result.Success();
    }

    private async Task<Result> CheckRoomAmenityType(
        int typeId,
        CancellationToken cancellationToken = default
    )
    {
        var maybeRoom = await _roomRepository.GetByIdAsync(typeId, cancellationToken);
        return maybeRoom.HasNoValue ? DomainErrors.Room.NotFound : Result.Success();
    }
}
