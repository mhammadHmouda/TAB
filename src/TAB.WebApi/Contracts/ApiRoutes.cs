namespace TAB.WebApi.Contracts;

public static class ApiRoutes
{
    public static class Auth
    {
        private const string Base = "auth";
        public const string Register = Base + "/register";
        public const string Activate = Base + "/activate";
        public const string Login = Base + "/login";
        public const string Logout = Base + "/logout";
    }

    public static class Users
    {
        private const string Base = "users";
        public const string Update = Base + "/{id}";
    }

    public static class Cities
    {
        private const string Base = "cities";
        public const string Create = Base;
        public const string UploadImages = Base + "/{id}/images";
        public const string Search = Base + "/search";
    }

    public static class Hotels
    {
        private const string Base = "hotels";
        public const string Create = Base;
        public const string UploadImages = Base + "/{id}/images";
        public const string Update = Base + "/{id}";
        public const string AddAmenity = Base + "/{id}/amenities";
        public const string CreateRoom = Base + "/{id}/rooms";
        public const string Search = Base + "/search";
        public const string Get = Base + "/{id}";
    }

    public static class Images
    {
        private const string Base = "images";
        public const string Upload = $"{Base}/references/{{id}}/type/{{type}}";
        public const string Update = $"{Base}/{{id}}";
        public const string Delete = $"{Base}/{{id}}";
    }

    public static class Amenities
    {
        private const string Base = "amenities";
        public const string Create = Base;
        public const string Update = Base + "/{id}";
        public const string Delete = Base + "/{id}";
    }

    public static class Rooms
    {
        private const string Base = "rooms";
        public const string AddDiscount = Base + "/{id}/discounts";
        public const string Update = Base + "/{id}";
        public const string Delete = Base + "/{id}";
    }

    public static class Review
    {
        private const string Base = "reviews";
        public const string Create = Base;
        public const string Delete = Base + "/{id}";
        public const string Update = Base + "/{id}";
    }
}
