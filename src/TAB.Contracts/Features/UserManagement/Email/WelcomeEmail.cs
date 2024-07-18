namespace TAB.Contracts.Features.UserManagement.Email;

public record WelcomeEmail(string Name, string EmailTo, string Token);
