using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Exceptions;

namespace TAB.Domain.Core.Shared;

public static class Ensure
{
    public static void NotEmpty(string value, string? message, string? argumentName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException(message, argumentName);
        }
    }

    public static void NotEmpty(Guid value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";

        if (value == Guid.Empty)
        {
            throw new DomainException(DomainErrors.General.NotEmpty.WithMessage(error));
        }
    }

    public static void NotEmpty(int value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";
        if (value == default)
        {
            throw new DomainException(DomainErrors.General.NotEmpty.WithMessage(error));
        }
    }

    public static void NotNull<TValue>(TValue value, string? message, string? argumentName)
        where TValue : class
    {
        var error = $"{argumentName} {message}";
        if (value is null)
        {
            throw new DomainException(DomainErrors.General.NotNull.WithMessage(error));
        }
    }
}
