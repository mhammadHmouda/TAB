namespace TAB.Domain.Core.Abstractions;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
