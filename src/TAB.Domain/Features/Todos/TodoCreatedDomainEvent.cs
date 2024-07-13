using TAB.Domain.Core.Primitives.Events;

namespace TAB.Domain.Features.Todos;

public record TodoCreatedDomainEvent(string Title) : IDomainEvent;
