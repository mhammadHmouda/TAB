using TAB.Domain.Core.Primitives;

namespace TAB.Domain.Features.Users.Entities;

public class Token : Entity
{
    public string Value { get; }
    public bool IsRevoked { get; private set; }
    public int UserId { get; }
    public DateTime ExpiresAt { get; }

    private Token(string value, bool isRevoked, int userId, DateTime expiresAt)
    {
        Value = value;
        IsRevoked = isRevoked;
        UserId = userId;
        ExpiresAt = expiresAt;
    }

    public static Token Create(string value, int userId, DateTime expiresAt) =>
        new(value, false, userId, expiresAt);

    public void Revoke() => IsRevoked = true;
}
