using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.UploadImages;

public class UploadImagesCommandHandler
    : ICommandHandler<UploadImagesCommand, Result<UploadImagesResponse>>
{
    private readonly IUploadFileService _uploadFileService;

    public UploadImagesCommandHandler(IUploadFileService uploadFileService) =>
        _uploadFileService = uploadFileService;

    public async Task<Result<UploadImagesResponse>> Handle(
        UploadImagesCommand request,
        CancellationToken cancellationToken
    )
    {
        var filesResult = FileHelper.ValidateFileRequest(request.Files);

        if (filesResult.IsFailure)
        {
            return filesResult.Error;
        }

        var fileResponses = await _uploadFileService.UploadFilesAsync(request.Files);
        var urls = fileResponses.Select(x => x.Url).ToArray();

        return new UploadImagesResponse(urls);
    }
}
