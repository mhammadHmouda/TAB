using TAB.Contracts.Features.Shared;

namespace TAB.WebApi.Extensions;

public static class FormCollectionExtensions
{
    public static FileRequest[] CreateFileRequest(this IFormCollection files) =>
        files
            .Files.Select(x => new FileRequest(
                x.FileName,
                x.ContentType,
                x.OpenReadStream().ReadAllBytes()
            ))
            .ToArray();

    public static FileRequest CreateFileRequest(this IFormFile file) =>
        new(file.FileName, file.ContentType, file.OpenReadStream().ReadAllBytes());
}
