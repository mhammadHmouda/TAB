using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Features.ReviewManagement.Events;

namespace TAB.Domain.Features.ReviewManagement.Entities;

public class Review : AggregateRoot, IAuditableEntity
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public int Rating { get; private set; }
    public int HotelId { get; }
    public int UserId { get; private set; }
    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private Review() { }

    private Review(string title, string content, int rating, int hotelId, int userId)
    {
        Title = title;
        Content = content;
        Rating = rating;
        HotelId = hotelId;
        UserId = userId;
    }

    public static Review Create(string title, string content, int rating, int hotelId, int userId)
    {
        Ensure.NotEmpty(title, nameof(title), "Review title is required.");
        Ensure.NotEmpty(content, nameof(content), "Review content is required.");
        Ensure.GreaterThan(rating, 0, nameof(rating), "Review rating must be greater than 0.");
        Ensure.LessThan(rating, 6, nameof(rating), "Review rating must be less than 6.");

        var review = new Review(title, content, rating, hotelId, userId);

        review.AddDomainEvent(new ReviewCreatedEvent(review.HotelId, review.Rating));

        return review;
    }
}
