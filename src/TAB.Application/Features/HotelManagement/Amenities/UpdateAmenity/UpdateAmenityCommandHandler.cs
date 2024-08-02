using AutoMapper;
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
    private readonly IMapper _mapper;

    public UpdateAmenityCommandHandler(
        IAmenityRepository amenityRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
