using System.Linq.Expressions;
using System.Reflection;

namespace TAB.Domain.Core.Utils;

public static class SortsParser<T>
{
    public static (Expression<Func<T, object>>?, bool) Parse(string sortString)
    {
        if (string.IsNullOrWhiteSpace(sortString))
            return (null, false);

        var isDescending = false;

        if (sortString.StartsWith("-"))
        {
            isDescending = true;
            sortString = sortString[1..];
        }

        var property = typeof(T).GetProperty(
            sortString,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance
        );

        if (property == null)
            return (null, false);

        var parameter = Expression.Parameter(typeof(T), "x");
        var member = Expression.Property(parameter, property);
        var conversion = Expression.Convert(member, typeof(object));
        var orderBy = Expression.Lambda<Func<T, object>>(conversion, parameter);

        return (orderBy, isDescending);
    }
}
