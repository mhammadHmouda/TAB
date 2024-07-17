using MediatR;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Activation;

public class UserActivationCommandHandler : IRequestHandler<UserActivationCommand, Result<Unit>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserActivationCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Unit>> Handle(
        UserActivationCommand request,
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
