using Microsoft.EntityFrameworkCore.Storage;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;

namespace TAB.Application.Core.Interfaces.Data;

public interface IUnitOfWork
{
    IRepository2<TEntity> Repository<TEntity>()
        where TEntity : Entity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );
}
