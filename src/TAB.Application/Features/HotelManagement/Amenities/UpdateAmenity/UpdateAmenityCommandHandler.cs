using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Amenities.UpdateAmenity;

public class UpdateAmenityCommandHandler
    : ICommandHandler<UpdateAmenityCommand, Result<AmenityResponse>>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateAmenityCommandHandler(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork)
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AmenityResponse>> Handle(
        UpdateAmenityCommand request,
        CancellationToken cancellationToken
    )
    {
        var amenityMaybe = await _amenityRepository.GetByIdAsync(request.Id, cancellationToken);

        if (amenityMaybe.HasNoValue)
        {
            return DomainErrors.Amenity.NotFound;
        }

        var amenity = amenityMaybe.Value;

        var result = amenity.Update(request.Name, request.Description);

        if (result.IsFailure)
        {
            return result.Error;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new AmenityResponse(amenity.Id, amenity.Name, amenity.Description);
    }
}
