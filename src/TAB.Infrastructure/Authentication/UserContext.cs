using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using TAB.Application.Core.Interfaces.Common;
using TAB.Domain.Features.UserManagement.Enums;

namespace TAB.Infrastructure.Authentication;

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContext(IHttpContextAccessor httpContextAccessor) =>
        _httpContextAccessor = httpContextAccessor;

    public int Id =>
        int.Parse(
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)!.Value
                ?? throw new ArgumentException(
                    "The user identifier claim is required.",
                    nameof(_httpContextAccessor)
                )
        );

    public UserRole Role =>
        Enum.Parse<UserRole>(
            _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)!.Value
                ?? throw new ArgumentException(
                    "The user role claim is required.",
                    nameof(_httpContextAccessor)
                )
        );

    public string Email =>
        _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Email)!.Value
        ?? throw new ArgumentException(
            "The user email claim is required.",
            nameof(_httpContextAccessor)
        );
}
