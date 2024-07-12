using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using TAB.Application.Core.Abstractions;
using TAB.Domain.Core.Primitives;

namespace TAB.Persistence;

public class TabDbContext : DbContext, IDbContext
{
    public TabDbContext(DbContextOptions<TabDbContext> options)
        : base(options) { }

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : Entity => base.Set<TEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public async Task<int> ExecuteSqlRawAsync(
        string sql,
        IEnumerable<SqlParameter> parameters,
        CancellationToken cancellationToken = default
    ) => await Database.ExecuteSqlRawAsync(sql, parameters, cancellationToken);

    public async Task<IDbContextTransaction> BeginTransactionAsync(
        CancellationToken cancellationToken = default
    ) => await Database.BeginTransactionAsync(cancellationToken);

    public IEnumerable<EntityEntry<TEntity>> GetChangedEntries<TEntity>()
        where TEntity : class => ChangeTracker.Entries<TEntity>();
}
