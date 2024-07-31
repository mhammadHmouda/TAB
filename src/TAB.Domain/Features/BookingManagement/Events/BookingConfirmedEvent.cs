using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.BookingManagement.Events;

public record BookingConfirmedEvent(int BookingId) : IDomainEvent;
