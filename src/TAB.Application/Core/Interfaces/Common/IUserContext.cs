using TAB.Domain.Features.UserManagement.Enums;

namespace TAB.Application.Core.Interfaces.Common;

public interface IUserContext
{
    int Id { get; }
    UserRole Role { get; }
    string Email { get; }
}
