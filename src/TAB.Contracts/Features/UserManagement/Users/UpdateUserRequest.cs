namespace TAB.Contracts.Features.UserManagement.Users;

public record UpdateUserRequest(int Id, string FirstName, string LastName, string? Password);
