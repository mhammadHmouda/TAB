using TAB.Application.Core.Interfaces.Common;

namespace TAB.Infrastructure.Common;

public class GuidTokenGenerator : ITokenGenerator
{
    public string Generate() => Guid.NewGuid().ToString().Replace("-", string.Empty);
}
