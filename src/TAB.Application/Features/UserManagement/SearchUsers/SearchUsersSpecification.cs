using TAB.Domain.Core.Specifications;
using TAB.Domain.Features.UserManagement.Entities;

namespace TAB.Application.Features.UserManagement.SearchUsers;

public class SearchUsersSpecification : BaseSpecification<User>
{
    public SearchUsersSpecification(int page, int pageSize, string? filters, string? sorting)
    {
        ApplyNoTracking();

        ApplyPaging(page, pageSize);
        AddDynamicFilters(filters);
        AddDynamicSorting(sorting);
    }
}
