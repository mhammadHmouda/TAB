using TAB.Domain.Core.Events;

namespace TAB.Domain.Features.Todos;

public record TodoCreatedDomainEvent(string Title) : IDomainEvent;
