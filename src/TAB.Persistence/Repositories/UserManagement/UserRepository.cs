using Microsoft.EntityFrameworkCore;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Shared.Maybe;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.UserManagement;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(IDbContext dbContext)
        : base(dbContext) { }

    public async Task<Maybe<User>> GetByEmailAsync(string email)
    {
        var user = await DbContext.Set<User>().FirstOrDefaultAsync(user => user.Email == email);

        return user ?? Maybe<User>.None;
    }
}
