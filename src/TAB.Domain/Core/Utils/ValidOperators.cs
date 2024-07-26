namespace TAB.Domain.Core.Utils;

public static class ValidOperators
{
    public static readonly string[] AllOperators =
    {
        CaseInsensitiveDoesNotStartsWith,
        CaseInsensitiveDoesNotContains,
        CaseInsensitiveNotEquals,
        CaseInsensitiveEquals,
        CaseInsensitiveStartsWith,
        CaseInsensitiveContains,
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
    public const string CaseInsensitiveContains = "@=*";
    public const string CaseInsensitiveStartsWith = "_=*";
    public const string CaseInsensitiveEquals = "==*";
    public const string CaseInsensitiveNotEquals = "!=*";
    public const string CaseInsensitiveDoesNotContains = "!@=*";
    public const string CaseInsensitiveDoesNotStartsWith = "!_=*";
}
