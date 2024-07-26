using AutoMapper;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Hotels;

public class HotelProfile : Profile
{
    public HotelProfile()
    {
        CreateMap<Hotel, HotelSearchResponse>()
            .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City.Name))
            .ForMember(
                dest => dest.NumberOfAvailableRooms,
                opt => opt.MapFrom(src => src.Rooms.Count(r => r.IsAvailable))
            );
    }
}
