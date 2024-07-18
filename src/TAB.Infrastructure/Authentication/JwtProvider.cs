using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TAB.Application.Core.Interfaces.Common;
using TAB.Domain.Core.Interfaces;
using TAB.Domain.Features.UserManagement.Entities;
using TAB.Infrastructure.Authentication.Options;

namespace TAB.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;
    private readonly IDateTimeProvider _dateTime;

    public JwtProvider(IOptions<JwtOptions> jwtOptions, IDateTimeProvider dateTime)
    {
        _dateTime = dateTime;
        _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey!));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email.Value),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var tokenExpirationDate = _dateTime.UtcNow.AddMinutes(_jwtOptions.TokenExpirationInMinutes);

        var tokenSettings = new JwtSecurityToken(
            _jwtOptions.ValidIssuer,
            _jwtOptions.ValidAudience,
            claims,
            expires: tokenExpirationDate,
            signingCredentials: credentials
        );

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.WriteToken(tokenSettings);
        user.AddToken(token, tokenExpirationDate);

        return token;
    }
}
