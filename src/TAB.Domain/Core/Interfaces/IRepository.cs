using System.Linq.Expressions;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Core.Specifications;

namespace TAB.Domain.Core.Interfaces;

public interface IRepository<TEntity>
    where TEntity : class
{
    Task<Maybe<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task<Maybe<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken
    );

    Task<Maybe<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken
    );

    Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    );

    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(TEntity entity);
    Task AddRangeAsync(IEnumerable<TEntity> entities);

    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);

    void Update(TEntity entity);

    Task<bool> AnyAsync(ISpecification<TEntity> specification);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    Task<int> CountAsync(ISpecification<TEntity> specification);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
}
