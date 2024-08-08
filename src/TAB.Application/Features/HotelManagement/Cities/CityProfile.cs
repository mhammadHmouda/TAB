using AutoMapper;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Cities;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, CityResponse>();
    }
}
