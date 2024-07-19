using MediatR;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Activation;

public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        ActivateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByAsync(
            user => user.ActivationCode.Value == request.Token,
            cancellationToken
        );

        if (user.HasNoValue)
        {
            return DomainErrors.User.UserAlreadyActive;
        }

        user.Value.Activate();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
