using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.UserManagement;

public class TokenRepository : BaseRepository<Token>, ITokenRepository
{
    public TokenRepository(IDbContext dbContext)
        : base(dbContext) { }
}
