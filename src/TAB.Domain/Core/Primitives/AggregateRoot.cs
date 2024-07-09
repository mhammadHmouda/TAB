﻿using TAB.Domain.Core.Events;

namespace TAB.Domain.Core.Primitives;

public class AggregateRoot : Entity
{
    protected AggregateRoot(int id)
        : base(id) { }

    protected AggregateRoot() { }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
