using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Contracts.Features.UserManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.Login;

public class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, Result<TokenResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public LoginUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<Result<TokenResponse>> Handle(
        LoginUserCommand request,
        CancellationToken cancellationToken
    )
    {
        var user = await _userRepository.GetByAsync(
            user => user.Email == request.Email,
            cancellationToken
        );

        if (user.HasNoValue)
        {
            return DomainErrors.User.EmailOrPasswordIncorrect;
        }

        var isPasswordValid = _passwordHasher.VerifyPassword(user.Value.Password, request.Password);

        if (!isPasswordValid)
        {
            return DomainErrors.User.EmailOrPasswordIncorrect;
        }

        if (!user.Value.IsActive)
        {
            return DomainErrors.User.UserNotActivated;
        }

        var token = _jwtProvider.GenerateToken(user.Value);

        return new TokenResponse(token);
    }
}
