using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Hotels.SearchHotels;

public record SearchHotelsQuery(string? Filters, string? Sorting, int Page, int PageSize)
    : IQuery<Result<PagedList<HotelSearchResponse>>>;
