namespace TAB.Contracts.Features.Shared;

public record FileRequest(string FileName, string ContentType, byte[] Content);
