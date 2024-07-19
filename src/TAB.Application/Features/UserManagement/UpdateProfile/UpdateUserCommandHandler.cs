using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace TAB.Application.Features.UserManagement.UpdateProfile;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUserContext _userContext;

    public UpdateUserCommandHandler(
        IPasswordHasher passwordHasher,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext
    )
    {
        _passwordHasher = passwordHasher;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
    }

    public async Task<Result> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        if (_userContext.Id != command.Id)
        {
            return DomainErrors.General.Unauthorized;
        }

        var maybeUser = await _userRepository.GetByIdAsync(command.Id, cancellationToken);

        if (maybeUser.HasNoValue)
        {
            return DomainErrors.User.UserNotFound;
        }

        var user = maybeUser.Value;

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            var updateNameResult = user.Update(command.FirstName, command.LastName, null);

            if (updateNameResult.IsFailure)
            {
                return updateNameResult.Error;
            }
        }
        else
        {
            var password = Password.Create(command.Password);

            if (password.IsFailure)
            {
                return password.Error;
            }

            var result = user.Update(
                command.FirstName,
                command.LastName,
                _passwordHasher.HashPassword(password.Value)
            );

            if (result.IsFailure)
            {
                return result.Error;
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
