using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.BookingManagement.Events;

public sealed record BookingCancelledEvent(int BookingId) : IDomainEvent;
