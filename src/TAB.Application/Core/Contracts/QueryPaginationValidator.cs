using System.Reflection;
using FluentValidation;

namespace TAB.Application.Core.Contracts;

public class QueryPaginationValidator<T> : AbstractValidator<T>
{
    public QueryPaginationValidator()
    {
        var type = typeof(T);

        var hasPageProperty =
            type.GetProperty("Page", BindingFlags.Public | BindingFlags.Instance) != null;
        var hasPageSizeProperty =
            type.GetProperty("PageSize", BindingFlags.Public | BindingFlags.Instance) != null;

        if (hasPageProperty && hasPageSizeProperty)
        {
            RuleFor(x => (int)type.GetProperty("Page")!.GetValue(x)!)
                .GreaterThan(0)
                .WithMessage("Page number must be greater than 0.");

            RuleFor(x => (int)type.GetProperty("PageSize")!.GetValue(x)!)
                .GreaterThan(0)
                .WithMessage("Page size must be greater than 0.");
        }
    }
}
