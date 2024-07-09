namespace TAB.WebApi.Contracts;

public static class ApiRoutes
{
    public static class Hello
    {
        private const string Base = "test";
        public const string Get = Base;
        public const string GetWithWelcome = Base + "/welcome";
    }
}
