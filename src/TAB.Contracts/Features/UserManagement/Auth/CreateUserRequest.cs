using TAB.Domain.Features.UserManagement.Enums;

namespace TAB.Contracts.Features.UserManagement.Auth;

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role
);
