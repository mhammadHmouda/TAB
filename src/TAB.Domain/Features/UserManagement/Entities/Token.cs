using TAB.Domain.Core.Primitives;

namespace TAB.Domain.Features.UserManagement.Entities;

public class Token : Entity
{
    public string Value { get; }
    public bool IsRevoked { get; private set; }
    public int UserId { get; }
    public DateTime ExpiresAt { get; }

    private Token(string value, int userId, DateTime expiresAt)
    {
        Value = value;
        IsRevoked = false;
        UserId = userId;
        ExpiresAt = expiresAt;
    }

    public static Token Create(string value, int userId, DateTime expiresAt) =>
        new(value, userId, expiresAt);

    public void Revoke() => IsRevoked = true;
}
