using System.Linq.Expressions;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Maybe;

namespace TAB.Domain.Core.Interfaces;

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
}
