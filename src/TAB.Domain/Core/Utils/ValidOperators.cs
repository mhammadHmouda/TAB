namespace TAB.Domain.Core.Utils;

public static class ValidOperators
{
    public static readonly string[] AllOperators =
    {
        DoesNotStartsWith,
        DoesNotContains,
        StartsWith,
        Contains,
        LessThanEqualTo,
        LessThan,
        GreaterThanEqualTo,
        GreaterThan,
        NotEquals,
        Equal
    };

    public const string Equal = "==";
    public const string NotEquals = "!=";
    public const string GreaterThan = ">";
    public const string LessThan = "<";
    public const string GreaterThanEqualTo = ">=";
    public const string LessThanEqualTo = "<=";
    public const string Contains = "@=";
    public const string DoesNotContains = "!@=";
    public const string StartsWith = "_=";
    public const string DoesNotStartsWith = "!_=";
}
