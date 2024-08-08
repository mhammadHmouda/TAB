using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Domain.Features.UserManagement.Repositories;

public interface ITokenRepository : IRepository<Token>
{
    void RemoveExpiredTokens();
}
