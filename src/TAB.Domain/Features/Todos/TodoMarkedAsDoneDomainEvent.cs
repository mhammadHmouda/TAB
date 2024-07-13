using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.Todos;

public record TodoMarkedAsDoneDomainEvent(Todo Todo) : IDomainEvent;
