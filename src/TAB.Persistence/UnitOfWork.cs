using System.Collections;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly IDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IMediator _mediator;
    private readonly Hashtable _repositories;

    public UnitOfWork(IDbContext dbContext, IDateTimeProvider dateTimeProvider, IMediator mediator)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _mediator = mediator;

        _repositories = new Hashtable();
    }

    public IRepository2<TEntity> Repository<TEntity>()
        where TEntity : Entity
    {
        var type = typeof(TEntity).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);

            var repositoryInstance = Activator.CreateInstance(
                repositoryType.MakeGenericType(typeof(TEntity)),
                _dbContext
            );

            _repositories.Add(type, repositoryInstance);
        }

        return (IRepository2<TEntity>)_repositories[type]!;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var utcNow = _dateTimeProvider.UtcNow;

        UpdateAuditableEntities(utcNow);
        await PublishDomainEvents(cancellationToken);

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    ) => await _dbContext.BeginTransactionAsync(cancellationToken);

    private void UpdateAuditableEntities(DateTime utcNow)
    {
        var auditableEntities = _dbContext
            .GetChangedEntries<IAuditableEntity>()
            .Where(entityEntry => entityEntry.State is EntityState.Added or EntityState.Modified)
            .ToList();

        auditableEntities.ForEach(entityEntry =>
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(nameof(IAuditableEntity.CreatedAtUtc)).CurrentValue = utcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(nameof(IAuditableEntity.UpdatedAtUtc)).CurrentValue = utcNow;
            }
        });
    }

    private async Task PublishDomainEvents(CancellationToken cancellationToken)
    {
        var aggregateRoots = _dbContext
            .GetChangedEntries<AggregateRoot>()
            .Where(entityEntry => entityEntry.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = aggregateRoots
            .SelectMany(entityEntry => entityEntry.Entity.DomainEvents)
            .ToList();

        aggregateRoots.ForEach(entityEntry => entityEntry.Entity.ClearDomainEvents());

        var tasks = domainEvents.Select(domainEvent =>
            _mediator.Publish(domainEvent, cancellationToken)
        );

        await Task.WhenAll(tasks);
    }
}
