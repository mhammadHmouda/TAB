using System.Text.RegularExpressions;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Domain.Features.Users;

public partial class Email : ValueObject
{
    private const string EmailRegexPattern =
        """^(?!\.)("([^"\r\\]|\\["\r\\])*"|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$""";

    public const int MaxLength = 254;

    private static readonly Lazy<Regex> EmailRegex = new(MyRegex);

    private Email(string value) => Value = value;

    public string Value { get; set; }

    public static Result<Email> Create(string email) =>
        Result
            .Create(email)
            .Ensure(e => !string.IsNullOrWhiteSpace(e), DomainErrors.Email.CannotBeEmpty)
            .Ensure(e => e.Length <= MaxLength, DomainErrors.Email.LongerThanAllowed)
            .Ensure(e => EmailRegex.Value.IsMatch(e), DomainErrors.Email.InvalidFormat)
            .Map(e => new Email(e));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Email From(string value)
    {
        Ensure.NotEmpty(value, "The email is required.", nameof(value));

        return new Email(value);
    }

    public static implicit operator string(Email email) => email.Value;

    public static explicit operator Email(string email) => From(email);

    [GeneratedRegex(EmailRegexPattern, RegexOptions.Compiled)]
    private static partial Regex MyRegex();
}
