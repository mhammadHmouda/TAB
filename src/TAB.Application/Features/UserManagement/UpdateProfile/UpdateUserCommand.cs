using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.UserManagement.UpdateProfile;

public record UpdateUserCommand(int Id, string FirstName, string LastName, string? Password)
    : ICommand<Result>;
