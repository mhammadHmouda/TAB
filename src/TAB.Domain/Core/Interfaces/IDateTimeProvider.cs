namespace TAB.Domain.Core.Interfaces;

public interface IDateTimeProvider
{
    public DateTime UtcNow { get; }
}
