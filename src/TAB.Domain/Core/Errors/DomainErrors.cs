using TAB.Domain.Core.Primitives;

namespace TAB.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error UserNotFound => new("user_not_found", "User not found.");
        public static Error UserAlreadyExists => new("user_already_exists", "User already exists.");
    }

    public static class General
    {
        public static Error NotNull => new("General.NotNull", "The value is not null.");

        public static Error NotEmpty => new("General.NotEmpty", "The value is not empty.");
    }
}
