using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Hotels.AddHotels;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.HotelManagement;

public class CreateHotelCommandTests
{
    private readonly ICityRepository _cityRepositoryMock;
    private readonly IUserRepository _userRepositoryMock;
    private readonly CreateHotelCommandHandler _handler;
    private readonly CreateHotelCommand _command;

    public CreateHotelCommandTests()
    {
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();
        var hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _cityRepositoryMock = Substitute.For<ICityRepository>();
        _userRepositoryMock = Substitute.For<IUserRepository>();

        _handler = new CreateHotelCommandHandler(
            hotelRepositoryMock,
            _cityRepositoryMock,
            _userRepositoryMock,
            unitOfWorkMock
        );

        _command = new CreateHotelCommand("Hotel", "Description", 1, 1, 1, 1, HotelType.Luxury);

        var city = City.Create("City", "Country", "PostOffice").Value;
        _cityRepositoryMock
            .GetByIdAsync(1, default)
            .Returns(Task.FromResult(Maybe<City>.From(city)));

        var owner = User.Create(
            Email.Create("test@example.com").Value,
            "Owner",
            "Owner",
            "validDDD1!",
            UserRole.User,
            ActivationCode.Create("activationCode", DateTime.Now.AddHours(2))
        );
        _userRepositoryMock
            .GetByIdAsync(1, default)
            .Returns(Task.FromResult(Maybe<User>.From(owner)));
    }

    [Fact]
    public async Task Handle_WhenHotelIsCreated_ReturnsHotelResponse()
    {
        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenCityDoesNotExist_ReturnsCityNotFoundError()
    {
        // Arrange
        _cityRepositoryMock.GetByIdAsync(1, default).Returns(Task.FromResult(Maybe<City>.None));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Hotel.CityNotFound);
    }

    [Fact]
    public async Task Handle_WhenUserDoesNotExist_ReturnsUserNotFoundError()
    {
        // Arrange
        _userRepositoryMock.GetByIdAsync(1, default).Returns(Task.FromResult(Maybe<User>.None));

        // Act
        var result = await _handler.Handle(_command, default);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Hotel.OwnerNotFound);
    }

    [Fact]
    public async Task Handle_WhenLatitudeOutOfRange_ReturnsLatitudeOutOfRangeError()
    {
        // Arrange
        var command = _command with
        {
            Latitude = 91
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.LatitudeOutOfRange);
    }

    [Fact]
    public async Task Handle_WhenLongitudeOutOfRange_ReturnsLongitudeOutOfRangeError()
    {
        // Arrange
        var command = _command with
        {
            Longitude = 181
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.LongitudeOutOfRange);
    }

    [Fact]
    public async Task Handle_WhenLatitudeIsNaN_ReturnsNullLatitudeError()
    {
        // Arrange
        var command = _command with
        {
            Latitude = double.NaN
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.NullLatitude);
    }

    [Fact]
    public async Task Handle_WhenLongitudeIsNaN_ReturnsNullLongitudeError()
    {
        // Arrange
        var command = _command with
        {
            Longitude = double.NaN
        };

        // Act
        var result = await _handler.Handle(command, default);

        // Assert
        result.Error.Should().Be(DomainErrors.Location.NullLongitude);
    }
}
