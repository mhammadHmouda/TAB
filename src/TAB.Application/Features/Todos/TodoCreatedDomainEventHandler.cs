using TAB.Domain.Core.Events;
using TAB.Domain.Features.Todos;

namespace TAB.Application.Features.Todos;

public class TodoCreatedDomainEventHandler : IDomainEventHandler<TodoCreatedDomainEvent>
{
    public Task Handle(TodoCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine("Todo Created Successfully: " + notification.Title);
        return Task.CompletedTask;
    }
}
