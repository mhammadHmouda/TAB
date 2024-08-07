using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Hotels.GetFeaturedDeals;

public record GetFeaturedDealsQuery(int Limit) : IQuery<Result<List<FeaturedDealsResponse>>>;
