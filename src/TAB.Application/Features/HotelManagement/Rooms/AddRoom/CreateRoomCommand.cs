using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;

namespace TAB.Application.Features.HotelManagement.Rooms.AddRoom;

public record CreateRoomCommand(
    int HotelId,
    int Number,
    string Description,
    decimal Price,
    string Currency,
    RoomType Type,
    int CapacityOfAdults,
    int CapacityOfChildren
) : ICommand<Result<RoomResponse>>;
