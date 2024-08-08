using TAB.Contracts.Features.Shared;

namespace TAB.Application.Core.Interfaces.Common;

public interface IGeneratorService
{
    public string GenerateUniqueFileName(string fileName);
    public string GenerateToken();
}
