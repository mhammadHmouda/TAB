using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Features.UserManagement.Login;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.Auth;

public class LoginUserCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;
    private readonly LoginUserCommandHandler _handler;
    private readonly User _user;
    private readonly LoginUserCommand _command;

    public LoginUserCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _jwtProvider = Substitute.For<IJwtProvider>();
        _handler = new LoginUserCommandHandler(_userRepository, _passwordHasher, _jwtProvider);

        _user = User.Create(
            Email.Create("default@example.com").Value,
            "DefaultFirstName",
            "DefaultLastName",
            "defaultPassword",
            UserRole.User,
            ActivationCode.Create("defaultActivationCode", DateTime.UtcNow.AddDays(1))
        );

        _command = new LoginUserCommand("valid@gmail.com", "validDDD1!");
    }

    private void SetUpUser(bool isActive = true, string? password = null)
    {
        if (isActive)
        {
            _user.Activate();
        }

        if (password != null)
        {
            _user.Update(_user.FirstName, _user.LastName, password);
        }

        _userRepository
            .GetByAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Maybe<User>.From(_user)));
    }

    [Fact]
    public async Task Handle_ShouldReturnEmailOrPasswordIncorrect_WhenEmailIsIncorrect()
    {
        // Arrange
        var command = _command with
        {
            Email = "invalid"
        };

        _userRepository
            .GetByAsync(Arg.Any<Expression<Func<User, bool>>>(), Arg.Any<CancellationToken>())
            .Returns(Task.FromResult(Maybe<User>.None));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().BeEquivalentTo(DomainErrors.User.EmailOrPasswordIncorrect);
    }

    [Fact]
    public async Task Handle_ShouldReturnEmailOrPasswordIncorrect_WhenPasswordIsIncorrect()
    {
        // Arrange
        var command = _command with
        {
            Password = "invalid"
        };

        SetUpUser(isActive: true, password: "correctPassword");

        _passwordHasher.VerifyPassword(_user.Password, command.Password).Returns(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Error.Should().BeEquivalentTo(DomainErrors.User.EmailOrPasswordIncorrect);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserNotActivated_WhenUserIsNotActive()
    {
        // Arrange
        SetUpUser(isActive: false);

        _passwordHasher.VerifyPassword(_user.Password, _command.Password).Returns(true);

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.Error.Should().BeEquivalentTo(DomainErrors.User.UserNotActivated);
    }

    [Fact]
    public async Task Handle_ShouldReturnTokenResponse_WhenUserIsValid()
    {
        // Arrange
        SetUpUser(isActive: true, password: "defaultPassword");

        _passwordHasher.VerifyPassword(_user.Password, _command.Password).Returns(true);
        _jwtProvider.GenerateToken(_user).Returns("token");

        // Act
        var result = await _handler.Handle(_command, CancellationToken.None);

        // Assert
        result.Value.Token.Should().Be("token");
    }
}
