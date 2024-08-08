using FluentValidation;
using TAB.Domain.Core.Shared;

namespace TAB.Application.Core.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithError<T, TProperty>(
        this IRuleBuilderOptions<T, TProperty> ruleBuilder,
        Error error
    ) =>
        error is null
            ? throw new ArgumentNullException(nameof(error), "The error is required")
            : ruleBuilder.WithErrorCode(error.Code).WithMessage(error.Message);
}
