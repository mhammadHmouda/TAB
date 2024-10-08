﻿using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Images.UploadImages;

public class UploadImagesCommandHandler
    : ICommandHandler<UploadImagesCommand, Result<UploadImagesResponse>>
{
    private readonly IUploadFileService _uploadFileService;
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UploadImagesCommandHandler(
        IUploadFileService uploadFileService,
        IImageRepository imageRepository,
        IUnitOfWork unitOfWork
    )
    {
        _uploadFileService = uploadFileService;
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

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

        var imagesResult = Image.CreateImages(urls, request.ImageType, request.ReferenceId);

        if (imagesResult.IsFailure)
        {
            return imagesResult.Error;
        }

        await _imageRepository.AddRangeAsync(imagesResult.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new UploadImagesResponse(urls);
    }
}
