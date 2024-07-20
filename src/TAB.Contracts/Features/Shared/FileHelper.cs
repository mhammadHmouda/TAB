using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Contracts.Features.Shared;

public static class FileHelper
{
    private const int MaxImageSize = 5 * 1024 * 1024; // 5 MB
    private static readonly string[] ImageTypes = { "image/jpeg", "image/png" };
    private const int MaxImageCount = 5;

    public static Result ValidateFileRequest(FileRequest[] files) =>
        Result
            .Create(files)
            .Ensure(x => x.Length is > 0 and <= MaxImageCount, DomainErrors.Image.InvalidImageCount)
            .Ensure(
                x => x.All(file => ImageTypes.Contains(file.ContentType)),
                DomainErrors.Image.InvalidImageType
            )
            .Ensure(
                x => x.All(file => file.Content.Length <= MaxImageSize),
                DomainErrors.Image.InvalidImageSize
            );

    public static Result ValidateFileRequest(FileRequest file) =>
        Result
            .Create(file)
            .Ensure(x => ImageTypes.Contains(x.ContentType), DomainErrors.Image.InvalidImageType)
            .Ensure(x => x.Content.Length <= MaxImageSize, DomainErrors.Image.InvalidImageSize);
}
