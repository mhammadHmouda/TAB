using Microsoft.EntityFrameworkCore.Storage;

namespace TAB.Application.Core.Abstractions;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );
}
