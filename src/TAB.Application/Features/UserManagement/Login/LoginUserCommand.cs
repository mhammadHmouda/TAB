using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.UserManagement;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.UserManagement.Login;

public record LoginUserCommand(string Email, string Password) : ICommand<Result<TokenResponse>>;
