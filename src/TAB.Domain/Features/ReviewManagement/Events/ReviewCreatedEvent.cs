using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.ReviewManagement.Events;

public sealed record ReviewCreatedEvent(int HotelId, int StarRating) : IDomainEvent;
