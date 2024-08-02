using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.ReviewManagement.AddReview;
using TAB.Contracts.Features.ReviewManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.ReviewManagement;

public class CreateReviewCommandTests
{
    private readonly IHotelRepository _hotelRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly CreateReviewCommand Command = new("Title", "Content", 5, 1, 1);

    private static readonly User User = User.Create(
        Email.From("test@example.com"),
        "First Name",
        "Last Name",
        "Password",
        UserRole.Admin,
        ActivationCode.Create("Code", DateTime.UtcNow.AddHours(2))
    );

    private static readonly Hotel Hotel = Hotel.Create(
        "Name",
        "Description",
        Location.Create(35.4, 35.5).Value,
        HotelType.Luxury,
        1,
        1
    );

    private readonly CreateReviewCommandHandler _sut;

    public CreateReviewCommandTests()
    {
        _hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        var mapperMock = Substitute.For<IMapper>();
        _sut = new CreateReviewCommandHandler(
            _hotelRepositoryMock,
            _userRepositoryMock,
            _unitOfWorkMock,
            mapperMock
        );

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);
    }

    [Fact]
    public async Task Handle_WhenHotelNotFound_ReturnsNotFound()
    {
        // Arrange
        _hotelRepositoryMock
            .GetByIdWithReviewsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Hotel>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.Hotel.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnsNotFound()
    {
        // Arrange
        _hotelRepositoryMock
            .GetByIdWithReviewsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.None);
        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Error.Should().Be(DomainErrors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthorized_ReturnsReview()
    {
        // Arrange
        _hotelRepositoryMock
            .GetByIdWithReviewsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.Value.Should().BeOfType<ReviewResponse>();
    }

    [Fact]
    public async Task Handle_WhenUserIsAuthorized_SaveChanges()
    {
        // Arrange
        _hotelRepositoryMock
            .GetByIdWithReviewsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Hotel);

        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);

        // Act
        await _sut.Handle(Command, CancellationToken.None);

        // Assert
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
