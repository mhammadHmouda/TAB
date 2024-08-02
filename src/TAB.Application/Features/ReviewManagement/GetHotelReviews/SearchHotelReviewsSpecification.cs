using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.ReviewManagement.Entities;

namespace TAB.Application.Features.ReviewManagement.GetHotelReviews;

public class SearchHotelReviewsSpecification : BaseSpecification<Review>
{
    public SearchHotelReviewsSpecification(
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

        AddCriteria(r => r.HotelId == hotelId);
    }
}
