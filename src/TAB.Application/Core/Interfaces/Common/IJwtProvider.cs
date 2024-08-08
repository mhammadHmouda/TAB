using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Application.Core.Interfaces.Common;

public interface IJwtProvider
{
    string GenerateToken(User user);
}
