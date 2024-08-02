using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Amenities.GetAmenities;

public record GetAmenitiesQuery(int Page, int PageSize, string? Filters, string? Sorting)
    : IQuery<Result<PagedList<AmenityResponse>>>;
