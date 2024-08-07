using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.BookingManagement.Events;

public sealed record BookingPaidEvent(int BookingId) : IDomainEvent;
