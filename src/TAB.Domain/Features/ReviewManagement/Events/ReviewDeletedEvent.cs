using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.ReviewManagement.Events;

public sealed record ReviewDeletedEvent(int HotelId) : IDomainEvent;
