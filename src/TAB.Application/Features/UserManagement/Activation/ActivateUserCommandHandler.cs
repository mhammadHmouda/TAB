using MediatR;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Activation;

public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByActivationTokenAsync(request.Token);

        if (user.HasNoValue)
        {
            return DomainErrors.User.UserAlreadyActive;
        }

        user.Value.Activate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
