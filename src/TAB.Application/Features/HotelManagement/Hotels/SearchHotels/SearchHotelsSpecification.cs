using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels.SearchHotels;

public class SearchHotelsSpecification : BaseSpecification<Hotel>
{
    public SearchHotelsSpecification(string? filters, string? sorting, int page, int pageSize)
    {
        ApplyNoTracking();

        ApplyPaging(page, pageSize);
        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);

        AddInclude(h => h.City);
        AddInclude(h => h.Owner);
        AddInclude(h => h.Rooms);
    }
}
