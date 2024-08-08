using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Common;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Amenities;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.Services;

namespace TAB.Application.Features.HotelManagement.Amenities.AddAmenity;

public class CreateAmenityCommandHandler
    : ICommandHandler<CreateAmenityCommand, Result<AmenityResponse>>
{
    private readonly IAmenityRepository _amenityRepository;
    private readonly IAmenityService _amenityService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserContext _userContext;
    private readonly IMapper _mapper;

    public CreateAmenityCommandHandler(
        IAmenityRepository amenityRepository,
        IUnitOfWork unitOfWork,
        IUserContext userContext,
        IAmenityService amenityService,
        IMapper mapper
    )
    {
        _amenityRepository = amenityRepository;
        _unitOfWork = unitOfWork;
        _userContext = userContext;
        _amenityService = amenityService;
        _mapper = mapper;
    }

    public async Task<Result<AmenityResponse>> Handle(
        CreateAmenityCommand request,
        CancellationToken cancellationToken
    )
    {
        var result = await _amenityService.CheckAmenityTypeAndUserOwnerShipAsync(
            _userContext.Id,
            request.TypeId,
            request.Type,
            cancellationToken
        );

        if (result.IsFailure)
        {
            return result.Error;
        }

        var amenity = Amenity.Create(
            request.Name,
            request.Description,
            request.Type,
            request.TypeId
        );

        await _amenityRepository.AddAsync(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AmenityResponse>(amenity);
    }
}
