namespace TAB.Application.Core.Interfaces.Common;

public interface ITokenValidationService
{
    Task<bool> ValidateTokenAsync(string token);
}
