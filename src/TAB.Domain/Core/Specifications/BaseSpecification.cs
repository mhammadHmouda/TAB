using System.Linq.Expressions;
using TAB.Domain.Core.Utils;

namespace TAB.Domain.Core.Specifications;

public class BaseSpecification<T> : ISpecification<T>
{
    public Expression<Func<T, bool>>? Criteria { get; private set; }
    public List<Expression<Func<T, object>>> Includes { get; private init; } = new();
    public List<string> IncludeStrings { get; } = new();
    public Expression<Func<T, object>>? OrderBy { get; private set; }
    public Expression<Func<T, object>>? OrderByDescending { get; private set; }
    public int Take { get; private set; }
    public int Skip { get; private set; }
    public bool IsPagingEnabled { get; private set; }
    public bool IsNoTracking { get; private set; }

    protected void AddCriteria(Expression<Func<T, bool>> criteria)
    {
        if (Criteria == null)
        {
            Criteria = criteria;
        }
        else
        {
            var newCriteria = Expression.Lambda<Func<T, bool>>(
                Expression.AndAlso(Criteria.Body, criteria.Body),
                Criteria.Parameters.First()
            );
            Criteria = newCriteria;
        }
    }

    protected void AddInclude(Expression<Func<T, object>> includeExpression)
    {
        Includes.Add(includeExpression);
    }

    protected void AddInclude(string includeString)
    {
        IncludeStrings.Add(includeString);
    }

    protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
    {
        OrderByDescending = orderByDescExpression;
    }

    protected void ApplyPaging(int page, int pageSize)
    {
        Skip = (page - 1) * pageSize;
        Take = pageSize;
        IsPagingEnabled = true;
    }

    protected void ApplyNoTracking()
    {
        IsNoTracking = true;
    }

    protected void AddDynamicFilters(string? filterString)
    {
        if (!string.IsNullOrEmpty(filterString))
        {
            var filter = FilterParser<T>.Parse(filterString);

            if (filter != null)
            {
                AddCriteria(filter);
            }
        }
    }

    protected void AddDynamicSorting(string? sortString)
    {
        if (!string.IsNullOrEmpty(sortString))
        {
            var (orderBy, isDescending) = SortsParser<T>.Parse(sortString);

            if (orderBy != null)
            {
                if (isDescending)
                {
                    AddOrderByDescending(orderBy);
                }
                else
                {
                    AddOrderBy(orderBy);
                }
            }
        }
    }

    public ISpecification<T> ForCounting()
    {
        var countSpec = new BaseSpecification<T>
        {
            Criteria = Criteria,
            IsPagingEnabled = false,
            IsNoTracking = true
        };

        return countSpec;
    }
}
