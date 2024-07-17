using TAB.Domain.Core.Interfaces;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Domain.Features.UserManagement.Repositories;

public interface IUserRepository : IRepository<User>
{
    Task<Maybe<User>> GetByEmailAsync(string email);
    Task<Maybe<User>> GetByActivationTokenAsync(string token);
}
