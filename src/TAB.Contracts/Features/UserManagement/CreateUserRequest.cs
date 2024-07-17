using TAB.Domain.Features.UserManagement.Enums;

namespace TAB.Contracts.Features.UserManagement;

public record CreateUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role
);
