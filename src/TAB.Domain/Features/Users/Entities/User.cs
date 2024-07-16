using System.Xml.Linq;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Features.Users.Enums;
using TAB.Domain.Features.Users.ValueObjects;

namespace TAB.Domain.Features.Users.Entities;

public class User : AggregateRoot, IAuditableEntity
{
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public string? FirstName { get; private set; }
    public string? LastName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Token> _tokens = new();
    public IReadOnlyCollection<Token> Tokens => _tokens.AsReadOnly();

    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private User(Email email, string firstName, string lastName, Password password, UserRole role)
    {
        Email = email;
        FirstName = firstName;
        LastName = lastName;
        Password = password;
        Role = role;
        IsActive = false;
    }

    public static User Create(
        Email email,
        string firstName,
        string lastName,
        Password password,
        UserRole role
    )
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
        Ensure.NotEmpty(email, "The email is required.", nameof(email));
        Ensure.NotEmpty(password, "The password is required", nameof(password));
        Ensure.NotDefault(role, "The role is required", nameof(role));

        return new User(email, firstName, lastName, password, role);
    }

    public void AddToken(string tokenValue, DateTime expiresAt)
    {
        var token = Token.Create(tokenValue, Id, expiresAt);
        _tokens.Add(token);
    }
}
