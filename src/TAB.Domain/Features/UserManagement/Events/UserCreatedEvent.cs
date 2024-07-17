using TAB.Domain.Core.Primitives.Events;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Domain.Features.UserManagement.Events;

public record UserCreatedEvent(User User) : IDomainEvent;
