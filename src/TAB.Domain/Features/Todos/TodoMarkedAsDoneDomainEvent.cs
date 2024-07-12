using TAB.Domain.Core.Events;

namespace TAB.Domain.Features.Todos;

public record TodoMarkedAsDoneDomainEvent(Todo Todo) : IDomainEvent;
