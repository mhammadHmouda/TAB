namespace TAB.WebApi.Contracts;

public static class ApiRoutes
{
    public static class Todos
    {
        private const string Base = "todos";
        public const string GetById = Base + "/{id}";
        public const string Create = Base;
    }
}
