using TAB.Application.Core.Interfaces.Notifications;
using TAB.Contracts.Features.Shared.Email;
using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.UserManagement.Events;

namespace TAB.Application.Features.UserManagement.Register;

public class UserCreatedEventHandler : IDomainEventHandler<UserCreatedEvent>
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
