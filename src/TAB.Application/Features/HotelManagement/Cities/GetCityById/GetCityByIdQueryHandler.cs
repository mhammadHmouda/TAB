using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Cities.GetCityById;

public class GetCityByIdQueryHandler : IQueryHandler<GetCityByIdQuery, Result<CityResponse>>
{
    private readonly ICityRepository _cityRepository;
    private readonly IMapper _mapper;

    public GetCityByIdQueryHandler(ICityRepository cityRepository, IMapper mapper)
    {
        _cityRepository = cityRepository;
        _mapper = mapper;
    }

    public async Task<Result<CityResponse>> Handle(
        GetCityByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        var cityMaybe = await _cityRepository.GetByIdAsync(request.Id, cancellationToken);

        if (cityMaybe.HasNoValue)
        {
            return DomainErrors.Hotel.CityNotFound;
        }

        return _mapper.Map<CityResponse>(cityMaybe);
    }
}
