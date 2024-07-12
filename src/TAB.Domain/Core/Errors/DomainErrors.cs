using TAB.Domain.Core.Primitives;

namespace TAB.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error UserNotFound => new("User.NotFound", "User not found.");
        public static Error UserAlreadyExists => new("User.AlreadyExist", "User already exists.");
    }

    public static class Todo
    {
        public static Error NotFound => new("Todo.NotFound", "Todo not found.");
        public static Error AlreadyDone => new("Todo.AlreadyDone", "Todo is already done.");
    }

    public static class General
    {
        public static Error ServerError =>
            new("General.ServerError", "The server encountered an unrecoverable error.");

        public static Error NotNull => new("General.NotNull", "The value is not null.");

        public static Error NotEmpty => new("General.NotEmpty", "The value is not empty.");
    }
}
