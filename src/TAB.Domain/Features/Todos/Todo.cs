using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Domain.Features.Todos;

public class Todo : AggregateRoot, IAuditableEntity
{
    public Todo() { }

    private Todo(string title, string description, bool isDone)
    {
        Title = title;
        Description = description;
        IsDone = isDone;
    }

    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsDone { get; private set; }

    public static Todo Create(string title, string description, bool isDone)
    {
        var todo = new Todo(title, description, isDone);
        todo.AddDomainEvent(new TodoCreatedDomainEvent(todo.Title));
        return todo;
    }

    public void Update(string title, string description, bool isDone)
    {
        Title = title;
        Description = description;
        IsDone = isDone;
    }

    public Result MarkAsDone()
    {
        if (IsDone)
        {
            return Result.Failure(DomainErrors.Todo.AlreadyDone);
        }

        IsDone = true;

        AddDomainEvent(new TodoMarkedAsDoneDomainEvent(this));

        return Result.Success();
    }

    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
}
