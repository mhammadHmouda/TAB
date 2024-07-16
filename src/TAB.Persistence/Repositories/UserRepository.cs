using TAB.Application.Core.Interfaces;
using TAB.Domain.Features.Users.Entities;
using TAB.Domain.Features.Users.Repositories;

namespace TAB.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IDbContext dbContext)
        : base(dbContext) { }
}
