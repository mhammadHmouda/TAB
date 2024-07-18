using MediatR;
using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.UserManagement.Email;
using TAB.Domain.Features.UserManagement.Events;

namespace TAB.Application.Features.UserManagement.Register;

public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEmailNotificationService _emailService;

    public UserCreatedEventHandler(IEmailNotificationService emailService) =>
        _emailService = emailService;

    public async Task Handle(UserCreatedEvent notification, CancellationToken cancellationToken)
    {
        var name = $"{notification.User.FirstName} {notification.User.LastName}";
        var email = notification.User.Email.Value;
        var token = notification.User.ActivationCode.Value;

        var welcomeEmail = new WelcomeEmail(name, email, token);

        await _emailService.SendWelcomeEmail(welcomeEmail);
    }
}
