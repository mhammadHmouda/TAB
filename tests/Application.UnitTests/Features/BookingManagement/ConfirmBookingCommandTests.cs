using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.BookingManagement.ConfirmBooking;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Enums;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.BookingManagement;

public class ConfirmBookingCommandTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly ConfirmBookingCommandHandler _sut;
    private readonly Booking _booking;

    private static readonly ConfirmBookingCommand Command = new(BookingId: 1);

    public ConfirmBookingCommandTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new ConfirmBookingCommandHandler(_bookingRepositoryMock, _unitOfWorkMock);

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
    public async Task Handle_WhenBookingIsAlreadyConfirmed_ShouldReturnAlreadyConfirmedError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _booking.Status = BookingStatus.Confirmed;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyConfirmed);
    }

    [Fact]
    public async Task Handle_WhenBookingIsCancelled_ShouldReturnIsCancelledError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _booking.Status = BookingStatus.Cancelled;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.IsCancelled);
    }

    [Fact]
    public async Task Handle_WhenBookingIsPaid_ShouldReturnIsPaidError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _booking.Status = BookingStatus.Paid;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.IsPaid);
    }

    [Fact]
    public async Task Handle_WhenBookingIsSuccessfullyConfirmed_ShouldReturnSuccess()
    {
        // Arrange
        _bookingRepositoryMock
            .GetByIdAsync(Command.BookingId, CancellationToken.None)
            .Returns(_booking);

        _booking.Status = BookingStatus.Pending;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _booking.Status.Should().Be(BookingStatus.Confirmed);
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
