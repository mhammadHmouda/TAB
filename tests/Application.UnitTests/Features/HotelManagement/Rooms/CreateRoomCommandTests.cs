using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Rooms.AddRoom;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.HotelManagement.Rooms;

public class CreateRoomCommandTests
{
    private readonly IHotelRepository _hotelRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly CreateRoomCommandHandler _sut;

    private static readonly CreateRoomCommand Command =
        new(1, 1, "Room Description", 100, "USD", RoomType.Single, 1, 0);

    public CreateRoomCommandTests()
    {
        _hotelRepositoryMock = Substitute.For<IHotelRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CreateRoomCommandHandler(_hotelRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenHotelDoesNotExist_ReturnsHotelNotFoundError()
    {
        _hotelRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Hotel>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Hotel.NotFound);
    }

    [Fact]
    public async Task Handle_WhenHotelExists_ReturnsRoomResponse()
    {
        _hotelRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Hotel.Create(
                    "Name",
                    "Description",
                    Location.Create(35, 35).Value,
                    HotelType.Luxury,
                    1,
                    1
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
