using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Application.Core.Interfaces.Data;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Errors;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Entities;
using TAB.Domain.Features.HotelManagement.Repositories;
using TAB.Domain.Features.HotelManagement.ValueObjects;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Hotels.AddHotels;

public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, Result<HotelResponse>>
{
    private readonly IHotelRepository _hotelRepository;
    private readonly ICityRepository _cityRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateHotelCommandHandler(
        IHotelRepository hotelRepository,
        ICityRepository cityRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
    {
        _hotelRepository = hotelRepository;
        _cityRepository = cityRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<HotelResponse>> Handle(
        CreateHotelCommand request,
        CancellationToken cancellationToken
    )
    {
        var city = await _cityRepository.GetByIdAsync(request.CityId, cancellationToken);

        if (city.HasNoValue)
        {
            return DomainErrors.Hotel.CityNotFound;
        }

        var owner = await _userRepository.GetByIdAsync(request.OwnerId, cancellationToken);

        if (owner.HasNoValue)
        {
            return DomainErrors.Hotel.OwnerNotFound;
        }

        var locationResult = Location.Create(request.Latitude, request.Longitude);

        if (locationResult.IsFailure)
        {
            return locationResult.Error;
        }

        var hotel = Hotel.Create(
            request.Name,
            request.Description,
            locationResult.Value,
            request.Type,
            request.CityId,
            request.OwnerId
        );

        await _hotelRepository.AddAsync(hotel);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<HotelResponse>(hotel);
    }
}
