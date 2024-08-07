using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Features.HotelManagement.ValueObjects;

namespace TAB.Domain.Features.HotelManagement.Entities;

public class Discount : Entity, IAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int DiscountPercentage { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public int RoomId { get; private set; }
    public Room Room { get; internal set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Discount() { }

    private Discount(
        string name,
        string description,
        int discountPercentage,
        DateTime start,
        DateTime end,
        int roomId
    )
    {
        Name = name;
        Description = description;
        DiscountPercentage = discountPercentage;
        StartDate = start;
        EndDate = end;
        RoomId = roomId;
    }

    public static Discount Create(
        string name,
        string description,
        int discountPercentage,
        DateTime start,
        DateTime end,
        int roomId
    )
    {
        Ensure.NotEmpty(name, "The discount name is required.", nameof(name));
        Ensure.NotEmpty(description, "The discount description is required.", nameof(description));
        Ensure.NotDefault(
            discountPercentage,
            "The discount percentage is required.",
            nameof(discountPercentage)
        );
        Ensure.Between(
            discountPercentage,
            0,
            100,
            "The discount percentage must be between 0 and 100.",
            nameof(discountPercentage)
        );
        Ensure.IsTrue(start < end, "The start date must be before the end date.", nameof(start));
        Ensure.NotPast(start, "The start date must be in the future.", nameof(start));
        Ensure.NotPast(end, "The end date must be in the future.", nameof(end));
        Ensure.NotDefault(roomId, "The room id is required.", nameof(roomId));

        return new Discount(name, description, discountPercentage, start, end, roomId);
    }

    public Money Apply(Money price)
    {
        var discountedPrice = price.Amount - (price.Amount * DiscountPercentage / 100);
        return Money.Create(discountedPrice, price.Currency);
    }
}
