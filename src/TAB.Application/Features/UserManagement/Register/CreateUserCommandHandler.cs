using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.UserManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace TAB.Application.Features.UserManagement.Register;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ITokenGenerator _tokenGenerator;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider,
        ITokenGenerator tokenGenerator
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<Result<UserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userExists = await _userRepository.GetByAsync(
            user => user.Email == request.Email,
            cancellationToken
        );

        if (userExists.HasValue)
        {
            return DomainErrors.User.UserAlreadyExists;
        }

        var emailResult = Email.Create(request.Email);
        var passwordResult = Password.Create(request.Password);

        var result = Result.Combine(emailResult, passwordResult);

        if (result.IsFailure)
        {
            return result.Error;
        }

        var passwordHash = _passwordHasher.HashPassword(passwordResult.Value);

        var activationCode = ActivationCode.Create(
            _tokenGenerator.Generate(),
            _dateTimeProvider.UtcNow.AddHours(24)
        );

        var user = User.Create(
            emailResult.Value,
            request.FirstName,
            request.LastName,
            passwordHash,
            request.Role,
            activationCode
        );

        await _userRepository.InsertAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.Email.Value, user.FirstName, user.LastName);
    }
}
