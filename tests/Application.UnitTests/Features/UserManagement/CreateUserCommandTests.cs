using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Application.Features.UserManagement.Register;
using TAB.Contracts.Features.Shared.Email;
using TAB.Contracts.Features.UserManagement.Users;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.UserManagement;

public class CreateUserCommandTests
{
    private readonly CreateUserCommandHandler _handler;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly User _user;
    private readonly CreateUserCommand _command;

    public CreateUserCommandTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        var passwordHasherMock = Substitute.For<IPasswordHasher>();
        var generatorMock = Substitute.For<IGeneratorService>();
        var emailNotificationServiceMock = Substitute.For<IEmailNotificationService>();
        var mapperMock = Substitute.For<IMapper>();
        var dateTimeMock = Substitute.For<IDateTimeProvider>();
        dateTimeMock.UtcNow.Returns(DateTime.UtcNow);

        _command = new CreateUserCommand(
            "default@example.com",
            "validDDD1!",
            "defaultFirstName",
            "defaultLastName",
            UserRole.User
        );

        _user = User.Create(
            Email.Create("default@example.com").Value,
            "defaultFirstName",
            "defaultLastName",
            "validDDD1!",
            UserRole.User,
            ActivationCode.Create("activationCode", dateTimeMock.UtcNow.AddHours(1))
        );

        passwordHasherMock.HashPassword(_command.Password).Returns(_command.Password);
        generatorMock.GenerateToken().Returns("token");
        emailNotificationServiceMock
            .SendWelcomeEmail(Arg.Any<WelcomeEmail>())
            .Returns(Task.CompletedTask);

        mapperMock
            .Map<UserResponse>(Arg.Any<User>())
            .Returns(new UserResponse(1, "email@gmail.com", "mohammad", "Hmoudah"));

        _handler = new CreateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            passwordHasherMock,
            dateTimeMock,
            generatorMock,
            mapperMock
        );
    }

    private void SetupUserRepositoryMock(Maybe<User> user)
    {
        _userRepositoryMock
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), default)
            .Returns(user);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_EmailIsNotValid()
    {
        SetupUserRepositoryMock(Maybe<User>.None);

        // Arrange
        var invalidCommand = _command with
        {
            Email = "invalid"
        };

        // Act
        var result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Email.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_EmailIsNotUnique()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.From(_user));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.UserAlreadyExists);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserResponse_When_CommandIsValid()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.None);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<UserResponse>();
    }

    [Fact]
    public async Task Handle_Should_CallUnitOfWork_When_CommandIsValid()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.None);

        // Act
        await _handler.Handle(_command, default);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task Handle_Should_CallRepository_WhenEmailIsUnique()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.None);

        // Act
        await _handler.Handle(_command, default);

        // Assert
        await _userRepositoryMock.Received(1).AddAsync(Arg.Any<User>());
    }

    public static IEnumerable<object[]> PasswordTestData()
    {
        yield return new object[] { string.Empty, DomainErrors.Password.NullOrEmpty };
        yield return new object[] { "invalid", DomainErrors.Password.ShorterThanAllowed };
        yield return new object[] { "invalidddd", DomainErrors.Password.MissingUppercase };
        yield return new object[] { "INVALIDDDD", DomainErrors.Password.MissingLowercase };
        yield return new object[] { "invalidDDD", DomainErrors.Password.MissingDigit };
        yield return new object[] { "invalidDDD1", DomainErrors.Password.MissingNonAlphanumeric };
    }

    [Theory]
    [MemberData(nameof(PasswordTestData))]
    public async Task Handle_Should_ReturnError_ForInvalidPasswords(
        string password,
        Error expectedError
    )
    {
        SetupUserRepositoryMock(Maybe<User>.None);

        // Arrange
        var invalidCommand = _command with
        {
            Password = password
        };

        // Act
        var result = await _handler.Handle(invalidCommand, default);

        // Assert
        result.Error.Should().Be(expectedError);
    }
}
