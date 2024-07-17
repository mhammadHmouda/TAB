using MediatR;
using TAB.Contracts.Features.UserManagement;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Enums;

namespace TAB.Application.Features.UserManagement.Register;

public record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    UserRole Role
) : IRequest<Result<UserResponse>>;
