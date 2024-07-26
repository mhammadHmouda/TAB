using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Persistence.Specifications.HotelManagement;

public class HotelsSearchSpecification : BaseSpecification<Hotel>
{
    public HotelsSearchSpecification(string? filters, string? sorting, int page, int pageSize)
    {
        ApplyNoTracking();
        ApplyPaging(page, pageSize);
        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);
        AddInclude(h => h.City);
        AddInclude(h => h.Rooms);
    }
}
