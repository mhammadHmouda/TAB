namespace TAB.Contracts.Features.UserManagement;

public record MailRequest(string EmailTo, string Subject, string Body);
