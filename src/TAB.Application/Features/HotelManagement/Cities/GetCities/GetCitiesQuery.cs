using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Cities.GetCities;

public record GetCitiesQuery(string? Filters, string? Sorting, int Page, int PageSize)
    : IQuery<Result<PagedList<CityResponse>>>;
