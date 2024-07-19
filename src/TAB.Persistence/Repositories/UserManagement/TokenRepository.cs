using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Domain.Features.UserManagement.Repositories;
using TAB.Persistence.Repositories.Abstractions;

namespace TAB.Persistence.Repositories.UserManagement;

public class TokenRepository : BaseRepository<Token>, ITokenRepository
{
    private readonly IDateTimeProvider _dateTime;

    public TokenRepository(IDbContext dbContext, IDateTimeProvider dateTime)
        : base(dbContext) => _dateTime = dateTime;

    public void RemoveExpiredTokens()
    {
        var expiredTokens = DbContext
            .Set<Token>()
            .Where(token => token.ExpiresAt < _dateTime.UtcNow || token.IsRevoked);

        DbContext.Set<Token>().RemoveRange(expiredTokens);
    }
}
