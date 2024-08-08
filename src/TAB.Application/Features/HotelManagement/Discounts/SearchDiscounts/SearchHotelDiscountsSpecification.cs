using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Discounts.SearchDiscounts;

public class SearchHotelDiscountsSpecification : BaseSpecification<Discount>
{
    public SearchHotelDiscountsSpecification(
        int hotelId,
        int page,
        int pageSize,
        string? filters,
        string? sorting
    )
    {
        ApplyNoTracking();

        ApplyPaging(page, pageSize);
        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);

        AddInclude(d => d.Room);

        AddCriteria(d => d.Room.HotelId == hotelId);
    }
}
