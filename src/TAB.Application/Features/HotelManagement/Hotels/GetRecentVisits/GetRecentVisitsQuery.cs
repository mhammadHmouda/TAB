using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Hotels;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.HotelManagement.Hotels.GetRecentVisits;

public record GetRecentVisitsQuery(int Limit) : IQuery<Result<List<RecentVisitResponse>>>;
