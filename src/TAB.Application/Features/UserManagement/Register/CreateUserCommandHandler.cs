using MediatR;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.UserManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace TAB.Application.Features.UserManagement.Register;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<UserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userExists = await _userRepository.GetByEmailAsync(request.Email);

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

        var user = User.Create(
            emailResult.Value,
            request.FirstName,
            request.LastName,
            passwordHash,
            request.Role
        );

        await _userRepository.InsertAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserResponse(user.Id, user.Email.Value, user.FirstName, user.LastName);
    }
}
