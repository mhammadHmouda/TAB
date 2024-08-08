using AutoMapper;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Features.BookingManagement.Entities;

namespace TAB.Application.Features.BookingManagement;

public class BookingProfile : Profile
{
    public BookingProfile()
    {
        CreateMap<Booking, BookingResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}
