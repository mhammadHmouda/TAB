using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Logout;

public class LogoutUserCommandHandler : ICommandHandler<LogoutUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _hasher;

    public LogoutUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher hasher
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hasher = hasher;
    }

    public async Task<Result> Handle(LogoutUserCommand request, CancellationToken cancellationToken)
    {
        var hashedToken = _hasher.HashPassword(request.Token);

        var user = await _userRepository.GetByTokenAsync(hashedToken);

        if (user.HasNoValue)
        {
            return DomainErrors.User.InvalidToken;
        }

        user.Value.ClearToken(hashedToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
