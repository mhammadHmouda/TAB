using FluentValidation;
using TAB.Application.Core.Contracts;

namespace TAB.Application.Features.HotelManagement.Discounts.SearchDiscounts;

public class SearchDiscountQueryValidator : QueryPaginationValidator<SearchDiscountQuery>
{
    public SearchDiscountQueryValidator()
    {
        RuleFor(x => x.HotelId).GreaterThan(0).WithMessage("The hotel id must be greater than 0.");
    }
}
