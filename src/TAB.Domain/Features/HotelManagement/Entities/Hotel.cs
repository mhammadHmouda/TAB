﻿using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Enums;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.ReviewManagement.Entities;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Domain.Features.HotelManagement.Entities;

public class Hotel : AggregateRoot, IAuditableEntity
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public Location Location { get; private set; }
    public HotelType Type { get; private set; }
    public int StarRating { get; private set; }
    public int RoomsNumber { get; private set; }
    public int CityId { get; private set; }
    public int OwnerId { get; private set; }
    public City City { get; internal set; }
    public User Owner { get; internal set; }
    public ICollection<Room> Rooms { get; } = new List<Room>();
    public ICollection<Review> Reviews { get; } = new List<Review>();
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Hotel() { }

    private Hotel(
        string name,
        string description,
        Location location,
        HotelType type,
        int cityId,
        int ownerId
    )
    {
        Name = name;
        Description = description;
        Location = location;
        Type = type;
        CityId = cityId;
        OwnerId = ownerId;
        RoomsNumber = 0;
        StarRating = 0;
    }

    public static Hotel Create(
        string name,
        string description,
        Location location,
        HotelType type,
        int cityId,
        int ownerId
    )
    {
        Ensure.NotEmpty(name, "The hotel name is required.", nameof(name));
        Ensure.NotEmpty(description, "The hotel description is required.", nameof(description));
        Ensure.NotNull(location, "The hotel location is required.", nameof(location));
        Ensure.NotDefault(type, "The hotel type is required.", nameof(type));
        Ensure.NotDefault(cityId, "The city id is required.", nameof(cityId));
        Ensure.NotDefault(ownerId, "The owner id is required.", nameof(ownerId));

        return new Hotel(name, description, location, type, cityId, ownerId);
    }

    public Result Update(string name, string description, Location location)
    {
        Ensure.NotEmpty(name, "The name is required.", nameof(name));
        Ensure.NotEmpty(description, "The description is required.", nameof(description));
        Ensure.NotDefault(location, "The location is required.", nameof(location));

        if (Name == name && Description == description && Location == location)
        {
            return DomainErrors.Hotel.NothingToUpdate;
        }

        Name = name;
        Description = description;
        Location = location;

        return Result.Success();
    }

    public void AddRoom(Room room)
    {
        Ensure.NotNull(room, "The room is required.", nameof(room));

        Rooms.Add(room);

        RoomsNumber = Rooms.Count;
    }

    public void AddReview(Review review)
    {
        Ensure.NotNull(review, "The review is required.", nameof(review));

        Reviews.Add(review);
        UpdateStarRating();
    }

    public void UpdateReview(int reviewId)
    {
        var review = Reviews.FirstOrDefault(x => x.Id == reviewId);

        if (review is null)
        {
            return;
        }

        UpdateStarRating();
    }

    public void UpdateStarRating()
    {
        StarRating = Reviews.Any() ? (int)Math.Round(Reviews.Average(x => x.Rating), 0) : 0;
    }

    public int? CalculateMaxDiscountPercentage() =>
        Rooms.SelectMany(r => r.Discounts).MaxBy(d => d.DiscountPercentage)?.DiscountPercentage;

    public Money? CalculateMinPrice() => Rooms.MinBy(r => r.Price.Amount)?.Price;
}
