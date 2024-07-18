using MediatR;
using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.UserManagement.Logout;

public record LogoutUserCommand(string Token) : ICommand<Result<Unit>>;
