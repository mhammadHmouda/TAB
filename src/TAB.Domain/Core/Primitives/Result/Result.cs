﻿using System.Diagnostics.CodeAnalysis;

namespace TAB.Domain.Core.Primitives.Result;

public class Result
{
    public Result(bool isSuccess, Error error)
    {
        if (isSuccess && !Equals(error, Error.None) || !isSuccess && Equals(error, Error.None))
        {
            throw new ArgumentException("Invalid error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public Error Error { get; }

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue?> Failure<TValue>(Error error) => new(default, false, error);
}
