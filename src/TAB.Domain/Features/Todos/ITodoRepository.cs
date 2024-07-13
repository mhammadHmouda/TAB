using TAB.Domain.Core.Shared.Maybe;

namespace TAB.Domain.Features.Todos;

public interface ITodoRepository
{
    Task<Maybe<Todo>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task InsertAsync(Todo todo);
    void Update(Todo todo);
    void Remove(Todo todo);
}
