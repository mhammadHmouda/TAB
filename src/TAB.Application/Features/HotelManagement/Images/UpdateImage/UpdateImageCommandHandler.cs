using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Images.UpdateImage;

public class UpdateImageCommandHandler
    : ICommandHandler<UpdateImageCommand, Result<UpdateImageResponse>>
{
    private readonly IUploadFileService _uploadFileService;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateImageCommandHandler(
        IUploadFileService uploadFileService,
        IImageRepository imageRepository,
        IUnitOfWork unitOfWork
    )
    {
        _uploadFileService = uploadFileService;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<UpdateImageResponse>> Handle(
        UpdateImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var imageMaybe = await _imageRepository.GetByIdAsync(request.Id, cancellationToken);

        if (imageMaybe.HasNoValue)
        {
            return DomainErrors.Image.ImageNotFound;
        }

        var fileResult = FileHelper.ValidateFileRequest(request.File);

        if (fileResult.IsFailure)
        {
            return fileResult.Error;
        }

        var fileResponse = await _uploadFileService.UploadFilesAsync(new[] { request.File });

        var image = imageMaybe.Value;

        image.UpdateUrl(fileResponse[0].Url);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateImageResponse(image.Url);
    }
}
