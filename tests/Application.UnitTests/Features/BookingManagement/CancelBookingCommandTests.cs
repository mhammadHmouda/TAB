using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.BookingManagement.CancelBooking;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Enums;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.BookingManagement;

public class CancelBookingCommandTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUserContext _userContextMock;
    private readonly IDateTimeProvider _dateTimeProviderMock;
    private readonly CancelBookingCommandHandler _sut;
    private readonly Booking _booking;

    private static readonly CancelBookingCommand Command = new(BookingId: 1);

    public CancelBookingCommandTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _userContextMock = Substitute.For<IUserContext>();
        _dateTimeProviderMock = Substitute.For<IDateTimeProvider>();
        var unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new CancelBookingCommandHandler(
            _bookingRepositoryMock,
            unitOfWorkMock,
            _dateTimeProviderMock,
            _userContextMock
        );

        _dateTimeProviderMock.UtcNow.Returns(DateTime.Now);

        _booking = Booking.Create(
            _dateTimeProviderMock.UtcNow.AddDays(1),
            _dateTimeProviderMock.UtcNow.AddDays(2),
            1,
            1,
            1,
            Money.Create(3200, "USD")
        );
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
    public async Task Handle_WhenBookingIsAlreadyCancelled_ShouldReturnAlreadyCancelledError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _booking.Status = BookingStatus.Cancelled;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyCancelled);
    }

    [Fact]
    public async Task Handle_WhenBookingIsConfirmed_ShouldReturnIsConfirmedError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _booking.Status = BookingStatus.Confirmed;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.IsConfirmed);
    }

    [Fact]
    public async Task Handle_WhenBookingIsPaid_ShouldReturnIsPaidError()
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
        result.Error.Should().Be(DomainErrors.Booking.IsPaid);
    }

    [Fact]
    public async Task Handle_WhenBookingCannotBeCancelledDueToTimeConstraint_ShouldReturnCannotCancelError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _dateTimeProviderMock.UtcNow.Returns(_booking.CheckInDate.AddDays(2));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.CannotCancel);
    }

    [Fact]
    public async Task Handle_WhenBookingIsSuccessfullyCancelled_ShouldReturnSuccess()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _userContextMock.Id.Returns(1); // Same user ID

        _dateTimeProviderMock.UtcNow.Returns(_booking.CheckInDate.AddDays(-1));

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}
