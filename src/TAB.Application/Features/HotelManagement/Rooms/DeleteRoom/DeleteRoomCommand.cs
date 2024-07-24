using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Rooms.DeleteRoom;

public record DeleteRoomCommand(int Id) : ICommand<Result>;
