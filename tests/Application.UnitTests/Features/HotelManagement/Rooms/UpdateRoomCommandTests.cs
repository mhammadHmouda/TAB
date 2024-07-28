using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.HotelManagement.Rooms;
using TAB.Application.Features.HotelManagement.Rooms.UpdateRoom;
using TAB.Contracts.Features.HotelManagement.Rooms;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.HotelManagement.Rooms;

public class UpdateRoomCommandTests
{
    private readonly IRoomRepository _roomRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;

    private static readonly UpdateRoomCommand Command =
        new(1, 1, 100, "USD", RoomType.Single, 1, 2);

    private readonly UpdateRoomCommandHandler _sut;

    public UpdateRoomCommandTests()
    {
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();
        var dateTimeProviderMock = Substitute.For<IDateTimeProvider>();

        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new RoomProfile());
        });

        var mapper = config.CreateMapper();

        _sut = new UpdateRoomCommandHandler(
            _roomRepositoryMock,
            _unitOfWorkMock,
            dateTimeProviderMock,
            mapper
        );
    }

    [Fact]
    public async Task Handle_WhenRoomDoesNotExist_ReturnsRoomNotFoundError()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomExists_ReturnsRoomResponse()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(1, "USD"),
                    "Room Description",
                    RoomType.Single,
                    10,
                    1,
                    2,
                    false
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_UpdatesRoom()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(1, "USD"),
                    "Room Description",
                    RoomType.Single,
                    10,
                    1,
                    2,
                    false
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<RoomResponse>();
    }

    [Fact]
    public async Task Handle_WhenRoomExists_SavesChanges()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(1, "USD"),
                    "Room Description",
                    RoomType.Single,
                    10,
                    1,
                    2,
                    false
                )
            );

        await _sut.Handle(Command, CancellationToken.None);

        await _unitOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenRoomExistsAndNothingToUpdate_ReturnsNothingToUpdateError()
    {
        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(
                Room.Create(
                    1,
                    Money.Create(100, "USD"),
                    "Room Description",
                    RoomType.Single,
                    10,
                    1,
                    2,
                    false
                )
            );

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.Error.Should().Be(DomainErrors.Room.NothingToUpdate);
    }
}
