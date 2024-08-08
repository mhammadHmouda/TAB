using TAB.Contracts.Features.Shared.Email;

namespace TAB.Application.Core.Interfaces.Email;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
