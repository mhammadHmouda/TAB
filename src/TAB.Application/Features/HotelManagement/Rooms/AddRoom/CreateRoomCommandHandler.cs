using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Application.Features.HotelManagement.Rooms.AddRoom;

public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Result<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RoomResponse>> Handle(
        CreateRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var price = Money.Create(request.Price, request.Currency);

        var room = Room.Create(
            request.Number,
            price,
            request.Description,
            request.Type,
            request.HotelId,
            request.CapacityOfAdults,
            request.CapacityOfChildren
        );

        await _roomRepository.InsertAsync(room);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new RoomResponse(
            room.Id,
            room.Number,
            room.Price,
            room.DiscountedPrice,
            room.Type,
            room.IsAvailable,
            room.AdultsCapacity,
            room.ChildrenCapacity
        );
    }
}
