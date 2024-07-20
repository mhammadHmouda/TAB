using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Images.DeleteImage;

public class DeleteImageCommandHandler : ICommandHandler<DeleteImageCommand, Result>
{
    private readonly IImageRepository _imageRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteImageCommandHandler(IImageRepository imageRepository, IUnitOfWork unitOfWork)
    {
        _imageRepository = imageRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var imageMaybe = await _imageRepository.GetByIdAsync(request.Id, cancellationToken);

        if (imageMaybe.HasNoValue)
        {
            return DomainErrors.Image.ImageNotFound;
        }

        var image = imageMaybe.Value;

        _imageRepository.Remove(image);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
