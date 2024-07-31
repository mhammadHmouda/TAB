namespace TAB.Contracts.Features.Shared.Email;

public record MailRequest(string EmailTo, string Subject, string Body);
