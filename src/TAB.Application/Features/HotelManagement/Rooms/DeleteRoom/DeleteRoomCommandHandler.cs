using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Rooms.DeleteRoom;

public class DeleteRoomCommandHandler : ICommandHandler<DeleteRoomCommand, Result>
{
    private readonly IRoomRepository _roomRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteRoomCommandHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    {
        _roomRepository = roomRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
    {
        var roomMaybe = await _roomRepository.GetByIdAsync(request.Id, cancellationToken);

        if (roomMaybe.HasNoValue)
        {
            return DomainErrors.Room.NotFound;
        }

        _roomRepository.Remove(roomMaybe.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
