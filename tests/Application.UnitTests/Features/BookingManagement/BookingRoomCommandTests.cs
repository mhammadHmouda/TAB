using AutoMapper;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.BookingManagement.AddBooking;
using TAB.Contracts.Features.BookingManagement;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace Application.UnitTests.Features.BookingManagement;

public class BookingRoomCommandTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IRoomRepository _roomRepositoryMock;

    private static readonly BookingRoomCommand Command =
        new(DateTime.Now, DateTime.Now.AddDays(1), 1);

    private static readonly User User = User.Create(
        Email.From("test@example.com"),
        "test",
        "name",
        "Test Password",
        UserRole.User,
        ActivationCode.Create("1234", DateTime.UtcNow.AddHours(1))
    );

    private static readonly Room Room = Room.Create(
        1,
        Money.Create(500, "usd"),
        "Test Room",
        RoomType.Cabana,
        1,
        1
    );

    private readonly BookingRoomCommandHandler _sut;

    public BookingRoomCommandTests()
    {
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _roomRepositoryMock = Substitute.For<IRoomRepository>();
        var bookingRepositoryMock = Substitute.For<IBookingRepository>();
        var userContextMock = Substitute.For<IUserContext>();
        var mapperMock = Substitute.For<IMapper>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();

        mapperMock
            .Map<BookingResponse>(Arg.Any<Booking>())
            .Returns(
                new BookingResponse(
                    1,
                    1,
                    1,
                    1,
                    DateTime.UtcNow,
                    DateTime.UtcNow,
                    Money.Create(120, "usd"),
                    "Pending"
                )
            );

        _sut = new BookingRoomCommandHandler(
            bookingRepositoryMock,
            _roomRepositoryMock,
            _userRepositoryMock,
            unitOfWorkMock,
            mapperMock,
            userContextMock
        );
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ReturnUserNotFoundError()
    {
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<User>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.User.UserNotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomNotFound_ReturnRoomNotFoundError()
    {
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);

        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Maybe<Room>.None);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Room.NotFound);
    }

    [Fact]
    public async Task Handle_WhenRoomNotAvailable_ReturnRoomNotAvailableError()
    {
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);

        Room.UpdateAvailability(false);

        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Room.NotAvailable);
    }

    [Fact]
    public async Task Handle_WhenRoomUpdateAvailabilitySucceeds_ReturnBookingResponse()
    {
        _userRepositoryMock
            .GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(User);

        Room.UpdateAvailability(true);

        _roomRepositoryMock
            .GetByIdWithDiscountsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(Room);

        var result = await _sut.Handle(Command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeOfType<BookingResponse>();
    }
}
