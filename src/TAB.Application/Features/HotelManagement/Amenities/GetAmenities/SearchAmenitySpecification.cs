using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Amenities.GetAmenities;

public class SearchAmenitySpecification : BaseSpecification<Amenity>
{
    public SearchAmenitySpecification(int page, int pageSize, string? filters, string? sorting)
    {
        ApplyNoTracking();

        ApplyPaging(page, pageSize);

        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);
    }
}
