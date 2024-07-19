﻿using TAB.Domain.Core.Shared;

namespace TAB.Domain.Core.Errors;

public static class DomainErrors
{
    public static class User
    {
        public static Error UserNotFound => new("User.NotFound", "User not found.");
        public static Error UserAlreadyExists => new("User.AlreadyExist", "User already exists.");
        public static Error UserAlreadyActive =>
            new("User.AlreadyActive", "User is already active.");
        public static Error ActivationCodeExpired =>
            new("User.ActivationCodeExpired", "The activation code has expired.");
        public static Error EmailOrPasswordIncorrect =>
            new("User.EmailOrPasswordIncorrect", "The email or password is incorrect.");
        public static Error UserNotActivated =>
            new("User.NotActivated", "The user account is not activated.");
        public static Error InvalidToken => new("User.InvalidToken", "The token is invalid.");
        public static Error PasswordUnchanged =>
            new("User.PasswordUnchanged", "The new password is the same as the old password.");
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
        public static Error NotFuture =>
            new("General.NotFuture", "The value is not in the future.");
        public static Error NotPast => new("General.NotPast", "The value is not in the past.");
        public static Error UnProcessableRequest =>
            new("General.UnProcessableRequest", "The request is unprocessable.");
        public static Error Unauthorized => new("General.Unauthorized", "Unauthorized.");
    }

    public static class Location
    {
        public static Error NullLatitude => new("Location.NullLatitude", "Latitude is required.");
        public static Error NullLongitude => new("Location.NulLongitude", "Longitude is required.");
        public static Error LatitudeOutOfRange =>
            new("Location.Latitude out of range", "Latitude must be between -90 and 90.");
        public static Error LongitudeOutOfRange =>
            new("Location.Longitude out of range", "Longitude must be between -180 and 180.");
    }

    public static class Hotel
    {
        public static Error CityNotFound => new("Hotel.CityNotFound", "City not found.");
        public static Error OwnerNotFound => new("Hotel.OwnerNotFound", "Owner not found.");
    }
}
