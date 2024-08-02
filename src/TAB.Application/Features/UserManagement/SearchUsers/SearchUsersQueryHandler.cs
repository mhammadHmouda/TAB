using AutoMapper;
using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.UserManagement.Users;
using TAB.Domain.Core.Shared.Result;
using TAB.Domain.Features.UserManagement.Repositories;

namespace TAB.Application.Features.UserManagement.SearchUsers;

public class SearchUsersQueryHandler
    : IQueryHandler<SearchUsersQuery, Result<PagedList<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public SearchUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<UserResponse>>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken
    )
    {
        var spec = new SearchUsersSpecification(
            request.Page,
            request.PageSize,
            request.Filters,
            request.Sorting
        );

        var users = await _userRepository.GetAllAsync(spec, cancellationToken);
        var totalCount = await _userRepository.CountAsync(spec);

        var usersResponse = _mapper.Map<UserResponse[]>(users);

        return PagedList<UserResponse>.Create(
            usersResponse.ToList(),
            request.Page,
            request.PageSize,
            totalCount
        );
    }
}
