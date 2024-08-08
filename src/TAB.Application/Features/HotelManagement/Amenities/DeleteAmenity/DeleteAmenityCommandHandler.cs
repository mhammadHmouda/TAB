using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Amenities.DeleteAmenity;

public class DeleteAmenityCommandHandler : ICommandHandler<DeleteAmenityCommand, Result>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(
        DeleteAmenityCommand request,
        CancellationToken cancellationToken
    )
    {
        var amenityMaybe = await _amenityRepository.GetByIdAsync(request.Id, cancellationToken);

        if (amenityMaybe.HasNoValue)
        {
            return DomainErrors.Amenity.NotFound;
        }

        _amenityRepository.Delete(amenityMaybe.Value);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
