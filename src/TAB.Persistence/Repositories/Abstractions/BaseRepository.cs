using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Core.Specifications;
using TAB.Persistence.Specifications;

namespace TAB.Persistence.Repositories.Abstractions;

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

    public async Task<IReadOnlyCollection<TEntity>> GetAllBySpecificationAsync(
        ISpecification<TEntity> spec,
        CancellationToken cancellationToken
    ) => await ApplySpecification(spec).ToListAsync(cancellationToken);

    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> spec)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        return SpecificationEvaluator<TEntity>.GetQuery(query, spec);
    }
}

public class GenericRepository<TEntity> : IRepository2<TEntity>
    where TEntity : Entity
{
    protected readonly IDbContext DbContext;

    public GenericRepository(IDbContext dbContext) => DbContext = dbContext;

    public async Task<Maybe<TEntity>> GetByIdAsync(int id, CancellationToken cancellationToken) =>
        await DbContext.Set<TEntity>().FindAsync(new object[] { id }, cancellationToken)
        ?? Maybe<TEntity>.None;

    public async Task<Maybe<TEntity>> GetAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken
    ) =>
        await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken)
        ?? Maybe<TEntity>.None;

    public async Task<Maybe<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) =>
        await DbContext.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken)
        ?? Maybe<TEntity>.None;

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        ISpecification<TEntity> specification,
        CancellationToken cancellationToken
    ) => await ApplySpecification(specification).ToListAsync(cancellationToken);

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken cancellationToken
    ) => await DbContext.Set<TEntity>().Where(predicate).ToListAsync(cancellationToken);

    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken) =>
        await DbContext.Set<TEntity>().ToListAsync(cancellationToken);

    public async Task AddAsync(TEntity entity) => await DbContext.Set<TEntity>().AddAsync(entity);

    public async Task AddRangeAsync(IEnumerable<TEntity> entities) =>
        await DbContext.Set<TEntity>().AddRangeAsync(entities);

    public void Delete(TEntity entity) => DbContext.Set<TEntity>().Remove(entity);

    public void DeleteRange(IEnumerable<TEntity> entities) =>
        DbContext.Set<TEntity>().RemoveRange(entities);

    public void Update(TEntity entity) => DbContext.Set<TEntity>().Update(entity);

    public async Task<bool> AnyAsync(ISpecification<TEntity> specification) =>
        await CountAsync(specification) > 0;

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate) =>
        await CountAsync(predicate) > 0;

    public async Task<int> CountAsync(ISpecification<TEntity> specification) =>
        await ApplySpecification(specification).CountAsync();

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) =>
        await DbContext.Set<TEntity>().CountAsync(predicate);

    protected IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
    {
        var query = DbContext.Set<TEntity>().AsQueryable();

        return SpecificationEvaluator<TEntity>.GetQuery(query, specification);
    }
}
