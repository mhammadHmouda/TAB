using AutoMapper;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Domain.Features.HotelManagement.Entities;

namespace TAB.Application.Features.HotelManagement.Images;

public class ImageProfile : Profile
{
    public ImageProfile()
    {
        CreateMap<Image, ImageResponse>();
    }
}
