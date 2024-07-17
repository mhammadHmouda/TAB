using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;

namespace TAB.Domain.Features.UserManagement.ValueObjects;

public class ActivationCode : ValueObject
{
    public string Value { get; set; }
    public DateTime ExpiresAtUtc { get; set; }

    private ActivationCode(string value, DateTime expiresAtUtc)
    {
        Value = value;
        ExpiresAtUtc = expiresAtUtc;
    }

    public static ActivationCode Create(string value, DateTime expiresAtUtc)
    {
        Ensure.NotEmpty(value, "The activation code is required.", nameof(value));
        Ensure.NotDefault(expiresAtUtc, "The expiration date is required.", nameof(expiresAtUtc));
        Ensure.NotPast(
            expiresAtUtc,
            "The expiration date must be in the future.",
            nameof(expiresAtUtc)
        );

        return new ActivationCode(value, expiresAtUtc);
    }

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
        yield return ExpiresAtUtc;
    }
}
