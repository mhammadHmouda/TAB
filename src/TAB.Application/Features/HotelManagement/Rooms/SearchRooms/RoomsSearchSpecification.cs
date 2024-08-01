using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Rooms.SearchRooms;

public class RoomsSearchSpecification : BaseSpecification<Room>
{
    public RoomsSearchSpecification(int page, int pageSize, string filters, string sorting)
    {
        ApplyNoTracking();
        ApplyPaging(page, pageSize);
        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);
        AddInclude(r => r.Discounts);
    }
}
