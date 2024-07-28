using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Hotels.GetHotelById;

public record GetHotelByIdQuery(int Id) : IQuery<Result<HotelSearchResponse>>;
