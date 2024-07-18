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
}
