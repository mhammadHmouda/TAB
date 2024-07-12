using TAB.Application.Features.Abstractions;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Primitives.Maybe;
using TAB.Domain.Core.Primitives.Result;
using TAB.Domain.Features.Todos;

namespace TAB.Application.Features.Todos;

public record TodoResponse(int Id, string Title, bool IsDone, string? Description);

public record GetTodoQuery(int Id) : IQuery<Result<TodoResponse?>>;

public class GetTodoQueryHandler : IQueryHandler<GetTodoQuery, Result<TodoResponse?>>
{
    private readonly ITodoRepository _todoRepository;

    public GetTodoQueryHandler(ITodoRepository todoRepository) => _todoRepository = todoRepository;

    public async Task<Result<TodoResponse?>> Handle(
        GetTodoQuery request,
        CancellationToken cancellationToken
    )
    {
        var todoMaybe = await _todoRepository.GetByIdAsync(request.Id, cancellationToken);

        return todoMaybe.Match(
            todo => new TodoResponse(todo.Id, todo.Title, todo.IsDone, todo.Description),
            () => Result.Failure<TodoResponse>(DomainErrors.Todo.NotFound)
        );
    }
}
