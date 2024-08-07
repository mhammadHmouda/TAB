using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.ReviewManagement.GetHotelReviews;

public record GetHotelReviewsQuery(
    int HotelId,
    int Page,
    int PageSize,
    string? Filters,
    string? Sorting
) : IQuery<Result<PagedList<ReviewResponse>>>;
