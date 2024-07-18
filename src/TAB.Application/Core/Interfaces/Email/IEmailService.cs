using TAB.Contracts.Features.UserManagement.Email;

namespace TAB.Application.Core.Interfaces.Email;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
