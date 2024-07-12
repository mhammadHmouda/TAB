﻿using FluentValidation.Results;
using TAB.Domain.Core.Primitives;

namespace TAB.Application.Core.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IEnumerable<ValidationFailure> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures
            .Distinct()
            .Select(failure => new Error(failure.ErrorCode, failure.ErrorMessage))
            .ToList();
    }

    public IReadOnlyCollection<Error> Errors { get; }
}
