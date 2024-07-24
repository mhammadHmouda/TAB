using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Application.Features.HotelManagement.Rooms.AddRoom;

public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Result<RoomResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(IHotelRepository hotelRepository, IUnitOfWork unitOfWork)
    {
        _hotelRepository = hotelRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RoomResponse>> Handle(
        CreateRoomCommand command,
        CancellationToken cancellationToken
    )
    {
        var hotelMaybe = await _hotelRepository.GetByIdAsync(command.HotelId, cancellationToken);

        if (hotelMaybe.HasNoValue)
        {
            return DomainErrors.Hotel.NotFound;
        }

        var price = Money.Create(command.Price, command.Currency);

        var room = Room.Create(
            command.Number,
            price,
            command.Description,
            command.Type,
            command.HotelId,
            command.CapacityOfAdults,
            command.CapacityOfChildren
        );

        hotelMaybe.Value.AddRoom(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoomResponse(
            room.Id,
            room.Number,
            room.Description,
            room.Price,
            room.DiscountedPrice,
            room.Type,
            room.IsAvailable,
            room.AdultsCapacity,
            room.ChildrenCapacity
        );
    }
}
