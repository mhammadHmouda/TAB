using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Enums;
using TAB.Domain.Features.BookingManagement.Events;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Domain.Features.BookingManagement.Entities;

public class Booking : AggregateRoot, IAuditableEntity
{
    public DateTime CheckInDate { get; }
    public DateTime CheckOutDate { get; }
    public Money TotalPrice { get; private set; }
    public BookingStatus Status { get; private set; }
    public int UserId { get; private set; }
    public int HotelId { get; private set; }
    public int RoomId { get; private set; }
    public Room Room { get; internal set; }
    public User User { get; internal set; }
    public Hotel Hotel { get; internal set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Booking() { }

    private Booking(
        DateTime checkInDate,
        DateTime checkOutDate,
        int userId,
        int hotelId,
        int roomId,
        decimal pricePerNight,
        string currency
    )
    {
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        UserId = userId;
        HotelId = hotelId;
        RoomId = roomId;
        Status = BookingStatus.Pending;

        CalculateTotalPrice(pricePerNight, currency);

        AddDomainEvent(
            new BookingCreatedEvent(userId, hotelId, CheckInDate, CheckOutDate, TotalPrice!)
        );
    }

    public static Booking Create(
        DateTime checkInDate,
        DateTime checkOutDate,
        int userId,
        int hotelId,
        int roomId,
        decimal pricePerNight,
        string currency
    )
    {
        Ensure.NotPast(
            checkInDate,
            "The check in date must be in the future.",
            nameof(checkInDate)
        );
        Ensure.NotPast(
            checkOutDate,
            "The check out date must be in the future.",
            nameof(checkOutDate)
        );
        Ensure.GreaterThan(
            checkOutDate,
            checkInDate,
            "The check out date must be greater than the check in date.",
            nameof(checkOutDate)
        );
        Ensure.NotDefault(pricePerNight, "The price per night is required.", nameof(pricePerNight));

        return new Booking(
            checkInDate,
            checkOutDate,
            userId,
            hotelId,
            roomId,
            pricePerNight,
            currency
        );
    }

    private void CalculateTotalPrice(decimal pricePerNight, string currency)
    {
        var totalNights = (CheckOutDate - CheckInDate).Days;
        var totalAmount = pricePerNight * totalNights;

        TotalPrice = Money.Create(totalAmount, currency);
    }

    public Result Confirm()
    {
        if (Status == BookingStatus.Confirmed)
        {
            return DomainErrors.Booking.AlreadyConfirmed;
        }

        if (Status == BookingStatus.Cancelled)
        {
            return DomainErrors.Booking.AlreadyCancelled;
        }

        Status = BookingStatus.Confirmed;
        AddDomainEvent(new BookingConfirmedEvent(Id));

        return Result.Success();
    }

    public Result Cancel(DateTime now)
    {
        if (Status == BookingStatus.Cancelled)
        {
            return DomainErrors.Booking.AlreadyCancelled;
        }

        if (Status == BookingStatus.Confirmed)
        {
            return DomainErrors.Booking.AlreadyConfirmed;
        }

        if (CheckInDate < now.AddDays(1))
        {
            return DomainErrors.Booking.CannotCancel;
        }

        Status = BookingStatus.Cancelled;

        AddDomainEvent(new BookingCancelledEvent(Id));

        return Result.Success();
    }
}
