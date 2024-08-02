using AutoMapper;
using TAB.Contracts.Features.UserManagement.Users;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Application.Features.UserManagement;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForMember(dest => dest.EmailValue, opt => opt.MapFrom(src => src.Email.Value));
    }
}
