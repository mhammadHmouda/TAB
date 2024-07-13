using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using TAB.Domain.Core.Primitives;

namespace TAB.Application.Core.Interfaces;

public interface IDbContext
{
    DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    Task<int> ExecuteSqlRawAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default
    );

    Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    );

    IEnumerable<EntityEntry<TEntity>> GetChangedEntries<TEntity>()
        where TEntity : class;
}
