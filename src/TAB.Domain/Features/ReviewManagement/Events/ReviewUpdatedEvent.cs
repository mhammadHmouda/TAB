using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.ReviewManagement.Events;

public sealed record ReviewUpdatedEvent(int HotelId, int ReviewId) : IDomainEvent;
