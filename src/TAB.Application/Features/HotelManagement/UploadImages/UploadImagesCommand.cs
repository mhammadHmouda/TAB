using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Images;
using TAB.Contracts.Features.Shared;
using TAB.Domain.Core.Enums;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.UploadImages;

public record UploadImagesCommand(int ReferenceId, ImageType ImageType, FileRequest[] Files)
    : ICommand<Result<UploadImagesResponse>>;
