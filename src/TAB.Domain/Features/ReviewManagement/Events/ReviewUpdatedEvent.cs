using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.ReviewManagement.Events;

public record ReviewUpdatedEvent(int HotelId, int ReviewId) : IDomainEvent;
