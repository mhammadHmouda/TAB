using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.UserManagement.Users;
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
    private readonly IGeneratorService _generator;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IDateTimeProvider dateTimeProvider,
        IGeneratorService generator,
        IMapper mapper
    )
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _dateTimeProvider = dateTimeProvider;
        _generator = generator;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var userExists = await _userRepository.GetAsync(
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
            _generator.GenerateToken(),
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

        await _userRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserResponse>(user);
    }
}
