using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Amenities.GetAmenities;

public class GetAmenitiesQueryHandler
    : IQueryHandler<GetAmenitiesQuery, Result<PagedList<AmenityResponse>>>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IMapper _mapper;

    public GetAmenitiesQueryHandler(IAmenityRepository amenityRepository, IMapper mapper)
    {
        _amenityRepository = amenityRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<AmenityResponse>>> Handle(
        GetAmenitiesQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchAmenitySpecification(
            request.Page,
            request.PageSize,
            request.Filters,
            request.Sorting
        );

        var amenities = await _amenityRepository.GetAllAsync(spec, cancellationToken);
        var totalCount = await _amenityRepository.CountAsync(spec.ForCounting());

        var responses = _mapper.Map<AmenityResponse[]>(amenities);

        return PagedList<AmenityResponse>.Create(
            responses.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
