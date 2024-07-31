using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.BookingManagement.Events;

public record BookingCancelledEvent(int BookingId) : IDomainEvent;
