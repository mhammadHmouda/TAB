using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Rooms.DeleteRoom;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.HotelManagement.Rooms;

public class DeleteRoomCommandTests
{
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly DeleteRoomCommand Command = new(1);

    private readonly DeleteRoomCommandHandler _sut;

    public DeleteRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new DeleteRoomCommandHandler(_roomRepositoryMock, _unitOfWorkMock);
    }

    [Fact]
    public async Task Handle_WhenRoomDoesNotExist_ReturnsRoomNotFoundError()
    {
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_ReturnsSuccess()
    {
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(1, "USD"),
                    "Room Description",
                    RoomType.Cabana,
                    1,
                    1,
                    2,
                    false
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_DeletesRoom()
    {
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(1, "USD"),
                    "Room Description",
                    RoomType.Cabana,
                    1,
                    1,
                    2,
                    false
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        _roomRepositoryMock.Received(1).Delete(Arg.Any<Room>());
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_SavesChanges()
    {
        _roomRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(1, "USD"),
                    "Room Description",
                    RoomType.Cabana,
                    1,
                    1,
                    2,
                    false
                )
            );

        await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
