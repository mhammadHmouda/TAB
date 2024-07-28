using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Features.BookingManagement.Enums;

namespace TAB.Domain.Features.BookingManagement.Entities;

public class Booking : AggregateRoot, IAuditableEntity
{
    public DateTime CheckInDate { get; private set; }
    public DateTime CheckOutDate { get; private set; }
    public decimal TotalPrice { get; private set; }
    public int UserId { get; private set; }
    public int HotelId { get; private set; }
    public int RoomId { get; private set; }
    public BookingStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Booking() { }

    private Booking(
        DateTime checkInDate,
        DateTime checkOutDate,
        int userId,
        int hotelId,
        int roomId,
        decimal pricePerNight
    )
    {
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        UserId = userId;
        HotelId = hotelId;
        RoomId = roomId;
        Status = BookingStatus.Pending;

        CalculateTotalPrice(pricePerNight);
    }

    public static Booking Create(
        DateTime checkInDate,
        DateTime checkOutDate,
        int userId,
        int hotelId,
        int roomId,
        decimal pricePerNight
    )
    {
        Ensure.NotDefault(checkInDate, "The check in date is required.", nameof(checkInDate));
        Ensure.NotDefault(checkOutDate, "The check out date is required.", nameof(checkOutDate));
        Ensure.NotDefault(userId, "The user id is required.", nameof(userId));
        Ensure.NotDefault(hotelId, "The hotel id is required.", nameof(hotelId));
        Ensure.NotDefault(roomId, "The room id is required.", nameof(roomId));

        return new Booking(checkInDate, checkOutDate, userId, hotelId, roomId, pricePerNight);
    }

    private void CalculateTotalPrice(decimal pricePerNight)
    {
        var totalNights = (CheckOutDate - CheckInDate).Days;

        TotalPrice = pricePerNight * totalNights;
    }
}
