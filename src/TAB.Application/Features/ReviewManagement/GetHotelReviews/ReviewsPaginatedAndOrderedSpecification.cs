using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.ReviewManagement.Entities;

namespace TAB.Application.Features.ReviewManagement.GetHotelReviews;

public class ReviewsPaginatedAndOrderedSpecification : BaseSpecification<Review>
{
    public ReviewsPaginatedAndOrderedSpecification(
        int hotelId,
        int page,
        int pageSize,
        string? filters,
        string? sorting
    )
    {
        ApplyNoTracking();

        AddCriteria(r => r.HotelId == hotelId);

        ApplyPaging(page, pageSize);

        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);
    }
}
