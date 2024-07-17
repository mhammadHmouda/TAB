using MediatR;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.UserManagement.Activation;

public record UserActivationCommand(string Token) : IRequest<Result<Unit>>;
