using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using TAB.Application.Core.Interfaces.Data;
using TAB.Application.Features.BookingManagement.SuccessPayment;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.BookingManagement.Enums;
using TAB.Domain.Features.BookingManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace Application.UnitTests.Features.BookingManagement;

public class SuccessPaymentCommandTests
{
    private readonly IBookingRepository _bookingRepositoryMock;
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly SuccessPaymentCommandHandler _sut;
    private readonly Booking _booking;

    private static readonly SuccessPaymentCommand Command = new(SessionId: "test-session-id");

    public SuccessPaymentCommandTests()
    {
        _bookingRepositoryMock = Substitute.For<IBookingRepository>();
        _unitOfWorkMock = Substitute.For<IUnitOfWork>();

        _sut = new SuccessPaymentCommandHandler(_bookingRepositoryMock, _unitOfWorkMock);

        _booking = Booking.Create(
            DateTime.Now.AddDays(1),
            DateTime.Now.AddDays(2),
            1,
            1,
            1,
            Money.Create(2800, "USD")
        );
        _booking.AddSessionId(Command.SessionId);
    }

    [Fact]
    public async Task Handle_WhenBookingDoesNotExist_ShouldReturnBookingNotFoundError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetAsync(Arg.Any<Expression<Func<Booking, bool>>>(), CancellationToken.None)
            .Returns(Maybe<Booking>.None);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotFound);
    }

    [Fact]
    public async Task Handle_WhenBookingIsNotConfirmed_ShouldReturnNotConfirmedError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetAsync(Arg.Any<Expression<Func<Booking, bool>>>(), CancellationToken.None)
            .Returns(_booking);

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.NotConfirmed);
    }

    [Fact]
    public async Task Handle_WhenBookingIsAlreadyPaid_ShouldReturnAlreadyPaidError()
    {
        // Arrange
        _bookingRepositoryMock
            .GetAsync(Arg.Any<Expression<Func<Booking, bool>>>(), CancellationToken.None)
            .Returns(_booking);

        _booking.Status = BookingStatus.Paid;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be(DomainErrors.Booking.AlreadyPaid);
    }

    [Fact]
    public async Task Handle_WhenBookingIsSuccessfullyPaid_ShouldReturnSuccess()
    {
        // Arrange
        _bookingRepositoryMock
            .GetAsync(Arg.Any<Expression<Func<Booking, bool>>>(), CancellationToken.None)
            .Returns(_booking);

        _booking.Status = BookingStatus.Confirmed;

        // Act
        var result = await _sut.Handle(Command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _booking.Status.Should().Be(BookingStatus.Paid);
        await _unitOfWorkMock.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
