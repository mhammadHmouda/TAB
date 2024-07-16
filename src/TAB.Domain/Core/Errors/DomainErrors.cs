using TAB.Domain.Core.Shared;

namespace TAB.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error UserNotFound => new("User.NotFound", "User not found.");
        public static Error UserAlreadyExists => new("User.AlreadyExist", "User already exists.");
    }

    public static class Email
    {
        public static Error CannotBeEmpty => new("User.CannotBeEmpty", "Email cannot be empty.");
        public static Error TooLong => new("User.TooLong", "Email is too long.");
        public static Error LongerThanAllowed =>
            new("Email.LongerThanAllowed", "The email is longer than allowed.");
        public static Error InvalidFormat =>
            new("Email.InvalidFormat", "The email format is invalid.");
    }

    public static class Password
    {
        public static Error NullOrEmpty => new("Password.NullOrEmpty", "The password is required.");

        public static Error ShorterThanAllowed =>
            new("Password.ShorterThanAllowed", "The password is shorter than allowed.");

        public static Error LongerThanAllowed =>
            new("Password.LongerThanAllowed", "The password is longer than allowed.");

        public static Error MissingLowercase =>
            new(
                "Password.MissingLowercase",
                "The password must contain at least one lowercase letter."
            );

        public static Error MissingUppercase =>
            new(
                "Password.MissingUppercase",
                "The password must contain at least one uppercase letter."
            );

        public static Error MissingDigit =>
            new("Password.MissingDigit", "The password must contain at least one digit.");

        public static Error MissingNonAlphanumeric =>
            new(
                "Password.MissingNonAlphanumeric",
                "The password must contain at least one non-alphanumeric character."
            );
    }

    public static class General
    {
        public static Error ServerError =>
            new("General.ServerError", "The server encountered an unrecoverable error.");

        public static Error NotNull => new("General.NotNull", "The value is not null.");

        public static Error NotEmpty => new("General.NotEmpty", "The value is not empty.");
        public static Error NotDefault => new("General.NotDefault", "The value is not default.");
    }
}
