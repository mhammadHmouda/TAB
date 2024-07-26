using AutoMapper;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Amenities;

public class AmenityProfile : Profile
{
    public AmenityProfile()
    {
        CreateMap<Amenity, AmenityResponse>();
    }
}
