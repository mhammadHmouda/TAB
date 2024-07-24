using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Application.Features.HotelManagement.Rooms.UpdateRoom;

public class UpdateRoomCommandHandler : ICommandHandler<UpdateRoomCommand, Result<RoomResponse>>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTime;

    public UpdateRoomCommandHandler(
        IRoomRepository roomRepository,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTime
    )
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    public async Task<Result<RoomResponse>> Handle(
        UpdateRoomCommand request,
        CancellationToken cancellationToken
    )
    {
        var roomMaybe = await _roomRepository.GetByIdWithDiscountsAsync(
            request.Id,
            cancellationToken
        );

        if (roomMaybe.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        var room = roomMaybe.Value;

        var result = room.Update(
            request.Number,
            Money.Create(request.Price, request.Currency),
            request.Type,
            request.CapacityOfAdults,
            request.CapacityOfChildren,
            _dateTime.UtcNow
        );

        if (result.IsFailure)
        {
            return result.Error;
        }

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
