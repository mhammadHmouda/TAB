using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TAB.Application.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Maybe;

namespace TAB.Persistence.Repositories;

public abstract class BaseRepository<TEntity> : IRepository<TEntity>
    where TEntity : Entity
{
    protected readonly IDbContext DbContext;

    protected BaseRepository(IDbContext dbContext) => DbContext = dbContext;

    public async Task<Maybe<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await DbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken)
        ?? Maybe<TEntity>.None;

    public async Task<Maybe<TEntity>> GetByAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) =>
        await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken)
        ?? Maybe<TEntity>.None;

    public async Task InsertAsync(TEntity entity) =>
        await DbContext.Set<TEntity>().AddAsync(entity);

    public async Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities) =>
        await DbContext.Set<TEntity>().AddRangeAsync(entities);

    public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    public void Remove(TEntity entity) => DbContext.Set<TEntity>().Remove(entity);

    public async Task<int> ExecuteSqlAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default
    ) => await DbContext.ExecuteSqlRawAsync(sql, parameters, cancellationToken);
}
