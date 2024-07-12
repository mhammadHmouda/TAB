using MediatR;
using Microsoft.Extensions.Logging;
using TAB.Application.Core.Abstractions;
using TAB.Application.Features.Abstractions;

namespace TAB.Application.Core.Behaviours;

public class TransactionBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehaviour<TRequest, TResponse>> _logger;

    public TransactionBehaviour(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehaviour<TRequest, TResponse>> logger
    )
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }

        await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("Transaction committed successfully.");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during transaction. Rolling back transaction.");
            await transaction.RollbackAsync(cancellationToken);

            throw;
        }
    }
}
