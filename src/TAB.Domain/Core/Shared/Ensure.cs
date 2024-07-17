﻿using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Exceptions;

namespace TAB.Domain.Core.Shared;

public static class Ensure
{
    public static void NotEmpty(string value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException(DomainErrors.General.NotEmpty.WithMessage(error));
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

    public static void NotDefault<TValue>(TValue value, string? message, string? argumentName)
    {
        var error = $"{argumentName} {message}";

        if (EqualityComparer<TValue>.Default.Equals(value, default))
        {
            throw new DomainException(DomainErrors.General.NotDefault.WithMessage(error));
        }
    }
}
