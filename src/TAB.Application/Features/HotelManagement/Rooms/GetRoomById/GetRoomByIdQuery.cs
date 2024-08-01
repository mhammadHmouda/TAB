using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Rooms.GetRoomById;

public record GetRoomByIdQuery(int Id) : IQuery<Result<RoomSearchResponse>>;
