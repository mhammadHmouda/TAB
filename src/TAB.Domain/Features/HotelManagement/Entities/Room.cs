using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.BookingManagement.Entities;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Domain.Features.HotelManagement.Entities;

public class Room : Entity, IAuditableEntity
{
    public int Number { get; private set; }
    public string Description { get; private set; }
    public Money Price { get; set; }
    public RoomType Type { get; private set; }
    public int AdultsCapacity { get; private set; }
    public int ChildrenCapacity { get; private set; }
    public bool IsAvailable { get; private set; }
    public int HotelId { get; }
    public ICollection<Discount> Discounts { get; } = new List<Discount>();
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Room() { }

    private Room(
        int number,
        string description,
        Money price,
        RoomType type,
        int hotelId,
        bool isAvailable,
        int capacityOfAdults,
        int capacityOfChildren
    )
    {
        Number = number;
        Description = description;
        Price = price;
        Type = type;
        HotelId = hotelId;
        IsAvailable = isAvailable;
        AdultsCapacity = capacityOfAdults;
        ChildrenCapacity = capacityOfChildren;
    }

    public static Room Create(
        int number,
        Money price,
        string description,
        RoomType type,
        int hotelId,
        int capacityOfAdults = 2,
        int capacityOfChildren = 0,
        bool isAvailable = true
    ) =>
        new(
            number,
            description,
            price,
            type,
            hotelId,
            isAvailable,
            capacityOfAdults,
            capacityOfChildren
        );

    public Result<Booking> BookRoom(int userId, DateTime checkInDate, DateTime checkOutDate)
    {
        if (!IsAvailable)
            return DomainErrors.Room.NotAvailable;

        var totalPrice = CalculateTotalPrice(checkInDate, checkOutDate);

        var booking = Booking.Create(checkInDate, checkOutDate, userId, HotelId, Id, totalPrice);

        UpdateAvailability(false);

        return booking;
    }

    public Result Update(
        int number,
        Money price,
        RoomType type,
        int adultsCapacity,
        int childrenCapacity
    )
    {
        if (
            Number == number
            && Price == price
            && Type == type
            && AdultsCapacity == adultsCapacity
            && ChildrenCapacity == childrenCapacity
        )
        {
            return DomainErrors.Room.NothingToUpdate;
        }

        Number = number;
        Price = price;
        Type = type;
        AdultsCapacity = adultsCapacity;
        ChildrenCapacity = childrenCapacity;

        return Result.Success();
    }

    public Result UpdateAvailability(bool isAvailable)
    {
        if (IsAvailable == isAvailable)
        {
            return DomainErrors.Room.NothingToUpdate;
        }

        IsAvailable = isAvailable;

        return Result.Success();
    }

    public Result AddDiscount(Discount discount)
    {
        if (Discounts.Any(x => x.Name == discount.Name))
        {
            return DomainErrors.Discount.AlreadyExists;
        }

        Discounts.Add(discount);

        return Result.Success();
    }

    public Money CalculatePrice(DateTime date)
    {
        var activeDiscounts = Discounts
            .Where(d => d.StartDate <= date && d.EndDate >= date)
            .ToList();

        var discountedPrice = activeDiscounts.Aggregate(
            Price,
            (current, discount) => discount.Apply(current)
        );

        return discountedPrice;
    }

    private Money CalculateTotalPrice(DateTime checkInDate, DateTime checkOutDate)
    {
        var totalNights = (checkOutDate - checkInDate).Days;
        var totalPrice = 0m;

        for (var i = 0; i < totalNights; i++)
        {
            var currentDate = checkInDate.AddDays(i);
            totalPrice += CalculatePrice(currentDate).Amount;
        }

        return Money.Create(totalPrice, Price.Currency);
    }
}
