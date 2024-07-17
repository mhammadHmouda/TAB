using TAB.Contracts.Features.UserManagement;

namespace TAB.Application.Core.Interfaces.Email;

public interface IEmailService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
