using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.Todos;

namespace TAB.Application.Features.Todos;

public record MarkAsDoneRequest(int Id);

public record MarkAsDoneCommand(int Id) : ICommand<Result>;

public class MarkAsDoneCommandHandler : ICommandHandler<MarkAsDoneCommand, Result>
{
    private readonly ITodoRepository _todoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MarkAsDoneCommandHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
    {
        _todoRepository = todoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(MarkAsDoneCommand request, CancellationToken cancellationToken)
    {
        var maybeTodo = await _todoRepository.GetByIdAsync(request.Id, cancellationToken);

        if (maybeTodo.HasNoValue)
        {
            return Result.Failure(DomainErrors.Todo.NotFound);
        }

        var todo = maybeTodo.Value;

        var result = todo.MarkAsDone();

        if (result.IsFailure)
        {
            return result;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
