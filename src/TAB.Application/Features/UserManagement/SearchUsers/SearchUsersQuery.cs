using TAB.Application.Core.Contracts;
using TAB.Contracts.Features.UserManagement.Users;
using TAB.Domain.Core.Shared.Result;

namespace TAB.Application.Features.UserManagement.SearchUsers;

public record SearchUsersQuery(int Page, int PageSize, string? Filters, string? Sorting)
    : IQuery<Result<PagedList<UserResponse>>>;
