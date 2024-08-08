using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Cities;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Cities.GetTrendingDestination;

public record GetTrendingDestinationQuery(int Limit)
    : IQuery<Result<TrendingDestinationResponse[]>>;
