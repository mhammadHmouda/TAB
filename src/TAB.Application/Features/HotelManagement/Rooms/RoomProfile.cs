using AutoMapper;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Rooms;

public class RoomProfile : Profile
{
    public RoomProfile()
    {
        CreateMap<Room, RoomResponse>();
    }
}
