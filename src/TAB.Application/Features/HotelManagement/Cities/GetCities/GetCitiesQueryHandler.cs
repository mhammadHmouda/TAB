using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Cities.GetCities;

public class GetCitiesQueryHandler : IQueryHandler<GetCitiesQuery, Result<PagedList<CityResponse>>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<CityResponse>>> Handle(
        GetCitiesQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchCitiesSpecification(
            request.Filters,
            request.Sorting,
            request.Page,
            request.PageSize
        );

        var cities = await _cityRepository.GetAllAsync(spec, cancellationToken);
        var totalCount = await _cityRepository.CountAsync(spec.ForCounting());

        var citiesResponse = _mapper.Map<CityResponse[]>(cities);

        return PagedList<CityResponse>.Create(
            citiesResponse.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
