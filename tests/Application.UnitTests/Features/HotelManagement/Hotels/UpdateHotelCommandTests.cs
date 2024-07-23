using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Hotels.UpdateHotels;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.HotelManagement.Hotels;

public class UpdateHotelCommandTests
{
    private readonly IHotelRepository _hotelRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly UpdateHotelCommandHandler _sut;
    private readonly UpdateHotelCommand _command;

    public UpdateHotelCommandTests()
    {
        _hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new UpdateHotelCommandHandler(
            _hotelRepositoryMock,
            unitOfWorkMock,
            _userContextMock
        );

        _command = new UpdateHotelCommand(1, "Hotel Name", "Hotel Description", 1, 1);
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsHotelNotFound()
    {
        // Arrange
        _hotelRepositoryMock
            .GetByIdAsync(_command.Id, Arg.Any<CancellationToken>())
            .Returns(Maybe<Hotel>.None);

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Hotel.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotOwner_ReturnsUnauthorized()
    {
        // Arrange
        var hotel = Hotel.Create(
            "Hotel Name",
            "Hotel Description",
            Location.Create(1, 1).Value,
            HotelType.Luxury,
            1,
            1
        );
        _hotelRepositoryMock.GetByIdAsync(_command.Id, Arg.Any<CancellationToken>()).Returns(hotel);
        _userContextMock.Id.Returns(2);

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.General.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenLocationIsOutOfRange_ReturnsLocationOutOfRange()
    {
        // Arrange
        var hotel = Hotel.Create(
            "Hotel Name",
            "Hotel Description",
            Location.Create(1, 1).Value,
            HotelType.Luxury,
            1,
            1
        );
        _hotelRepositoryMock.GetByIdAsync(_command.Id, Arg.Any<CancellationToken>()).Returns(hotel);
        _userContextMock.Id.Returns(1);

        var command = _command with { Latitude = 91 };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Location.LatitudeOutOfRange);
    }

    [Fact]
    public async Task Handle_WhenNothingToUpdate_ReturnsNothingToUpdate()
    {
        // Arrange
        var hotel = Hotel.Create(
            "Hotel Name",
            "Hotel Description",
            Location.Create(1, 1).Value,
            HotelType.Luxury,
            1,
            1
        );
        _hotelRepositoryMock.GetByIdAsync(_command.Id, Arg.Any<CancellationToken>()).Returns(hotel);
        _userContextMock.Id.Returns(1);

        // Act
        var result = await _sut.Handle(_command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Hotel.NothingToUpdate);
    }

    [Fact]
    public async Task Handle_WhenUpdateIsSuccessful_ReturnsSuccess()
    {
        // Arrange
        var hotel = Hotel.Create(
            "Hotel Name",
            "Hotel Description",
            Location.Create(1, 1).Value,
            HotelType.Luxury,
            1,
            1
        );
        _hotelRepositoryMock.GetByIdAsync(_command.Id, Arg.Any<CancellationToken>()).Returns(hotel);
        _userContextMock.Id.Returns(1);

        var command = _command with { Name = "New Hotel Name" };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
