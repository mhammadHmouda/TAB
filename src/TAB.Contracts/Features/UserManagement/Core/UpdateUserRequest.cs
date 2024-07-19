namespace TAB.Contracts.Features.UserManagement.Core;

public record UpdateUserRequest(int Id, string FirstName, string LastName, string? Password);
