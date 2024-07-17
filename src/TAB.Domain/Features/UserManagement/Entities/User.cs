using MediatR;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Primitives;
using TAB.Domain.Core.Shared;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Enums;
using TAB.Domain.Features.UserManagement.Events;
using TAB.Domain.Features.UserManagement.ValueObjects;

namespace TAB.Domain.Features.UserManagement.Entities;

public class User : AggregateRoot, IAuditableEntity
{
    public Email Email { get; private set; }
    public string Password { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public UserRole Role { get; private set; }
    public bool IsActive { get; private set; }

    private readonly List<Token> _tokens = new();
    public IReadOnlyCollection<Token> Tokens => _tokens.AsReadOnly();

    public ActivationCode ActivationCode { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; internal set; }
    public DateTime? UpdatedAtUtc { get; internal set; }

    private User(Email email, string firstName, string lastName, string password, UserRole role)
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
        string password,
        UserRole role,
        ActivationCode activationCode
    )
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
        Ensure.NotEmpty(email, "The email is required.", nameof(email));
        Ensure.NotEmpty(password, "The password is required", nameof(password));
        Ensure.NotDefault(role, "The role is required", nameof(role));
        Ensure.NotEmpty(
            activationCode.Value,
            "The activation code is required",
            nameof(activationCode)
        );

        var user = new User(email, firstName, lastName, password, role);
        user.SetActivationCode(activationCode);

        user.AddDomainEvent(new UserCreatedEvent(user));

        return user;
    }

    public Result<Unit> Activate()
    {
        if (IsActive)
        {
            return DomainErrors.User.UserAlreadyActive;
        }

        if (ActivationCode.ExpiresAtUtc < DateTime.UtcNow)
        {
            return DomainErrors.User.ActivationCodeExpired;
        }

        IsActive = true;
        ActivationCode.Value = string.Empty;
        ActivationCode.ExpiresAtUtc = DateTime.MinValue;

        return Unit.Value;
    }

    public void AddToken(string tokenValue, DateTime expiresAt)
    {
        var token = Token.Create(tokenValue, Id, expiresAt);
        _tokens.Add(token);
    }

    private void SetActivationCode(ActivationCode activationCode)
    {
        ActivationCode = activationCode ?? throw new ArgumentNullException(nameof(activationCode));
    }
}
