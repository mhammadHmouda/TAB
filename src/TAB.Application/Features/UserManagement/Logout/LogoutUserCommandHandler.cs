using MediatR;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Logout;

public class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(
        LogoutUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByTokenAsync(request.Token);

        if (user.HasNoValue)
        {
            return DomainErrors.User.InvalidToken;
        }

        user.Value.ClearToken(request.Token);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
