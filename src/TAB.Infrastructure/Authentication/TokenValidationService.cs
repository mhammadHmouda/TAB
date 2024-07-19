using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Cryptography;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Infrastructure.Authentication;

public class TokenValidationService : ITokenValidationService
{
    private readonly IDateTimeProvider _dateTime;
    private readonly ITokenRepository _tokenRepository;
    private readonly IPasswordHasher _hasher;

    public TokenValidationService(
        IDateTimeProvider dateTime,
        ITokenRepository tokenRepository,
        IPasswordHasher hasher
    )
    {
        _dateTime = dateTime;
        _tokenRepository = tokenRepository;
        _hasher = hasher;
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        var hashedToken = _hasher.HashPassword(token);

        var tokenEntity = await _tokenRepository.GetByAsync(
            t => t.Value == hashedToken,
            CancellationToken.None
        );

        return tokenEntity is { HasNoValue: false, Value.IsRevoked: false }
            && tokenEntity.Value.ExpiresAt >= _dateTime.UtcNow;
    }
}
