using FluentValidation;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.Todos;

namespace TAB.Application.Features.Todos;

public record CreateTodoRequest(string Title, string Description, bool IsDone);

public record CreateTodoResponse(int Id);

public record CreateTodoCommand(string Title, string Description, bool IsDone)
    : ICommand<Result<CreateTodoResponse>>;

public class CreateTodoCommandHandler
    : ICommandHandler<CreateTodoCommand, Result<CreateTodoResponse>>
{
    private readonly ITodoRepository _todoRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTodoCommandHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork)
    {
        _todoRepository = todoRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CreateTodoResponse>> Handle(
        CreateTodoCommand request,
        CancellationToken cancellationToken
    )
    {
        var todo = Todo.Create(request.Title, request.Description, request.IsDone);
        await _todoRepository.InsertAsync(todo);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CreateTodoResponse(todo.Id);
    }
}

public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("Title is required.");
        RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
    }
}
