using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Cities.GetCityById;

public record GetCityByIdQuery(int Id) : IQuery<Result<CityResponse>>;
