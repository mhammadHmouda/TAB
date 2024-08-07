using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.HotelManagement.Discounts;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.HotelManagement.Repositories;

namespace TAB.Application.Features.HotelManagement.Discounts.SearchDiscounts;

public class SearchDiscountQueryHandler
    : IQueryHandler<SearchDiscountQuery, Result<PagedList<DiscountResponse>>>
{
    private readonly IDiscountRepository _discountRepository;
    private readonly IMapper _mapper;

    public SearchDiscountQueryHandler(IMapper mapper, IDiscountRepository discountRepository)
    {
        _mapper = mapper;
        _discountRepository = discountRepository;
    }

    public async Task<Result<PagedList<DiscountResponse>>> Handle(
        SearchDiscountQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchHotelDiscountsSpecification(
            request.HotelId,
            request.Page,
            request.PageSize,
            request.Filters,
            request.Sorting
        );

        var discounts = await _discountRepository.GetAllAsync(spec, cancellationToken);
        var totalCount = await _discountRepository.CountAsync(spec.ForCounting());

        var discountResponse = _mapper.Map<DiscountResponse[]>(discounts);

        return PagedList<DiscountResponse>.Create(
            discountResponse.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
