using FluentValidation;
using TAB.Application.Core.Contracts;

namespace TAB.Application.Features.ReviewManagement.GetHotelReviews;

public class GetReviewsQueryValidator : QueryPaginationValidator<GetHotelReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        RuleFor(x => x.HotelId).GreaterThan(0).WithMessage("The hotel id is required.");
    }
}
