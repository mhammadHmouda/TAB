using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Discounts.SearchDiscounts;

public record SearchDiscountQuery(
    int HotelId,
    int Page,
    int PageSize,
    string? Filters,
    string? Sorting
) : IQuery<Result<PagedList<DiscountResponse>>>;
