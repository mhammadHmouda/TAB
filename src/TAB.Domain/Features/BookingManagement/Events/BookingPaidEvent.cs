using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.BookingManagement.Events;

public record BookingPaidEvent(int BookingId) : IDomainEvent;
