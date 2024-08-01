using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Rooms.SearchRooms;

public record SearchRoomsQuery(int Page, int PageSize, string? Filters, string? Sorting)
    : IQuery<Result<PagedList<RoomSearchResponse>>>;
