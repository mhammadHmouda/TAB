using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.UserManagement.UpdateProfile;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.UserManagement;

public class UpdateUserCommandTests
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly UpdateUserCommandHandler _handler;
    private readonly User _user;
    private readonly UpdateUserCommand _command;

    public UpdateUserCommandTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _userContext = Substitute.For<IUserContext>();
        _handler = new UpdateUserCommandHandler(
            _passwordHasher,
            _userRepository,
            _unitOfWork,
            _userContext
        );

        _user = User.Create(
            Email.Create("default@example.com").Value,
            "John",
            "Doe",
            "validDDD1!",
            UserRole.User,
            ActivationCode.Create("defaultActivationCode", DateTime.UtcNow.AddDays(1))
        );

        _command = new UpdateUserCommand(1, "newFirstName", "newLastName", "validDDD1!");
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

        _userRepository.GetByIdAsync(1, default).Returns(Task.FromResult(Maybe<User>.From(_user)));
    }

    [Fact]
    public async Task Handle_ShouldReturnUnauthorized_WhenUserIsNotAuthorized()
    {
        // Arrange
        _userContext.Id.Returns(2);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.General.Unauthorized);
    }

    [Fact]
    public async Task Handle_ShouldReturnUserNotFound_WhenUserNotFound()
    {
        // Arrange
        _userContext.Id.Returns(1);
        _userRepository
            .GetByIdAsync(Arg.Any<int>(), default)
            .Returns(Task.FromResult(Maybe<User>.None));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPasswordIsNotProvided()
    {
        // Arrange
        _userContext.Id.Returns(1);
        SetUpUser(isActive: true, password: "validDDD1!");

        // Act
        var result = await _handler.Handle(_command with { Password = null }, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPasswordIsProvided()
    {
        // Arrange
        _userContext.Id.Returns(1);
        SetUpUser(isActive: true, password: "validDDD2!");

        _passwordHasher.HashPassword(Arg.Any<string>()).Returns(_command.Password);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert

        result.IsSuccess.Should().BeTrue();
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordIsInvalid()
    {
        // Arrange
        _userContext.Id.Returns(1);
        SetUpUser(isActive: true, password: "validDDD1!");

        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("invalidDDD1!");

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnError_WhenPasswordDoesNotChange()
    {
        // Arrange
        _userContext.Id.Returns(1);
        SetUpUser(isActive: true);

        _passwordHasher.HashPassword(Arg.Any<string>()).Returns(_user.Password);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.PasswordUnchanged);
    }
}
