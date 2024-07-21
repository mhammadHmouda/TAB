using TAB.Application.Core.Interfaces.Common;
using TAB.Domain.Core.Interfaces;

namespace TAB.Infrastructure.Common;

public class GeneratorService : IGeneratorService
{
    private readonly IDateTimeProvider _dateTime;

    public GeneratorService(IDateTimeProvider dateTime) => _dateTime = dateTime;

    public string GenerateUniqueFileName(string fileName) =>
        $"{_dateTime.UtcNow:yyyy_MM_dd__HH_mm_}{GenerateToken()}{fileName}";

    public string GenerateToken() => Guid.NewGuid().ToString().Replace("-", string.Empty);
}
