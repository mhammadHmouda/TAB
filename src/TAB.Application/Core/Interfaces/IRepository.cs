using System.Linq.Expressions;
using Microsoft.Data.SqlClient;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Maybe;

namespace TAB.Application.Core.Interfaces;

public interface IRepository<TEntity>
    where TEntity : Entity
{
    Task<Maybe<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<Maybe<TEntity>> GetByAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );
    Task InsertAsync(TEntity entity);
    Task InsertRangeAsync(IReadOnlyCollection<TEntity> entities);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> ExecuteSqlAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default
    );
}
