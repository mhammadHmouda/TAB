using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Core.Interfaces.Payment;
using TAB.Application.Features.BookingManagement.CheckoutRoom;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Enums;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.BookingManagement;

public class CheckoutBookingCommandTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly ISessionService _sessionServiceMock;
    private readonly IUserContext _userContextMock;
    private readonly CheckoutBookingCommandHandler _sut;
    private readonly Booking _booking;

    private static readonly CheckoutBookingCommand Command = new(BookingId: 1);

    public CheckoutBookingCommandTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _sessionServiceMock = Substitute.For<ISessionService>();
        _userContextMock = Substitute.For<IUserContext>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CheckoutBookingCommandHandler(
            _bookingRepositoryMock,
            _sessionServiceMock,
            _userContextMock,
            unitOfWorkMock
        );

        _booking = Booking.Create(
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            1,
            1,
            1,
            Money.Create(120, "USD"),
            new List<Discount>()
        );
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotExist_ShouldReturnBookingNotFoundError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(Maybe<Booking>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotFound);
    }

    [Fact]
    public async Task Handle_WhenUserIsNotTheOwnerOfTheBooking_ShouldReturnUnauthorizedError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(2); // Different user ID

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.General.Unauthorized);
    }

    [Fact]
    public async Task Handle_WhenBookingIsNotConfirmed_ShouldReturnNotConfirmedError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _booking.Status = BookingStatus.Pending;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotConfirmed);
    }

    [Fact]
    public async Task Handle_WhenBookingIsPaid_ShouldReturnAlreadyPaidError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _booking.Status = BookingStatus.Paid;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyPaid);
    }

    [Fact]
    public async Task Handle_WhenSessionServiceFails_ShouldReturnSessionServiceError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _booking.Status = BookingStatus.Confirmed;

        _sessionServiceMock
            .SaveAsync(Command.BookingId, CancellationToken.None)
            .Returns(DomainErrors.General.ServerError);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.General.ServerError);
    }

    [Fact]
    public async Task Handle_WhenCheckoutIsSuccessful_ShouldReturnSession()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _booking.Status = BookingStatus.Confirmed;

        var session = new Session("session_123", "publish_key");

        _sessionServiceMock.SaveAsync(Command.BookingId, CancellationToken.None).Returns(session);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(session);
    }
}
