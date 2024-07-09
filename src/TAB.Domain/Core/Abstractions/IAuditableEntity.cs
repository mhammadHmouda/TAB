namespace TAB.Domain.Core.Abstractions;

public interface IAuditableEntity
{
    DateTime CreatedAtUtc { get; }
    DateTime? UpdatedAtUtc { get; }
}
