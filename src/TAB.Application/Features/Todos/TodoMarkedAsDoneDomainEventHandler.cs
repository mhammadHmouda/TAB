using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.Todos;

namespace TAB.Application.Features.Todos;

public class TodoMarkedAsDoneDomainEventHandler : IDomainEventHandler<TodoMarkedAsDoneDomainEvent>
{
    public Task Handle(TodoMarkedAsDoneDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Todo with Id {domainEvent.Todo.Id} has been marked as done.");
        return Task.CompletedTask;
    }
}
