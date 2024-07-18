namespace TAB.Contracts.Features.UserManagement.Email;

public record MailRequest(string EmailTo, string Subject, string Body);
