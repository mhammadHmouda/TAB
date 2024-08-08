using TAB.Application.Core.Contracts;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.UserManagement.Activation;

public record ActivateUserCommand(string Token) : ICommand<Result>;
