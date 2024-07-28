using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.UserManagement.Activation;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.UserManagement;

public class ActivateUserCommandTests
{
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IDateTimeProvider _dateTimeMock;
    private readonly ActivateUserCommandHandler _handler;
    private readonly User _user;
    private readonly ActivateUserCommand _command;

    public ActivateUserCommandTests()
    {
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _dateTimeMock = Substitute.For<IDateTimeProvider>();

        _handler = new ActivateUserCommandHandler(
            _userRepositoryMock,
            _unitOfWorkMock,
            _dateTimeMock
        );

        _command = new ActivateUserCommand("token");

        _user = User.Create(
            Email.From("test@example.com"),
            "John",
            "Doe",
            "validDDD1!",
            UserRole.User,
            ActivationCode.Create("token", DateTime.UtcNow.AddHours(1))
        );
    }

    private void SetupUserRepositoryMock(Maybe<User> user)
    {
        _userRepositoryMock
            .GetAsync(Arg.Any<Expression<Func<User, bool>>>(), default)
            .Returns(user);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserNotFound()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.None);

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_UserIsAlreadyActive()
    {
        // Arrange
        _user.Activate();
        SetupUserRepositoryMock(Maybe<User>.From(_user));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.User.UserAlreadyActive);
    }

    [Fact]
    public async Task Handle_Should_ReturnError_When_TokenExpired()
    {
        // Arrange
        var now = DateTime.UtcNow.AddDays(2);
        _dateTimeMock.UtcNow.Returns(now);

        SetupUserRepositoryMock(Maybe<User>.From(_user));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result
            .Error.Should()
            .BeEquivalentTo(
                DomainErrors.User.ActivationCodeExpired,
                options => options.Excluding(error => error.Timestamp)
            );
    }

    [Fact]
    public async Task Handle_Should_ActivateUser()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.From(_user));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_SaveChanges()
    {
        // Arrange
        SetupUserRepositoryMock(Maybe<User>.From(_user));

        // Act
        await _handler.Handle(_command, default);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync();
    }
}
