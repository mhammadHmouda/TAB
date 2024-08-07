using AutoMapper;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Features.ReviewManagement.Entities;

namespace TAB.Application.Features.ReviewManagement;

public class ReviewProfile : Profile
{
    public ReviewProfile()
    {
        CreateMap<Review, ReviewResponse>();
    }
}
