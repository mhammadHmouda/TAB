using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Cities.GetCities;

public class CitiesSearchSpecification : BaseSpecification<City>
{
    public CitiesSearchSpecification(
        string? requestFilters,
        string? requestSorting,
        int requestPage,
        int requestPageSize
    )
    {
        ApplyNoTracking();
        ApplyPaging(requestPage, requestPageSize);
        AddDynamicSorting(requestSorting);
        AddDynamicFilters(requestFilters);
    }
}
