namespace TAB.Contracts.Features.Shared.Email;

public record WelcomeEmail(string Name, string EmailTo, string Token);
