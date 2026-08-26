using HrApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> GetQuery<T>(
        IQueryable<T> iputQuery,
        ISpecification<T> specification)
        where T : class
    {
        var query = iputQuery;

        if (specification.AsNoTracking)
        {
            query = query.AsNoTracking();
        }
        if(specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        query = specification.Includes.Aggregate(
            query,
            static (current, include) =>
                current.Include(include));
        if(specification.OrderBy is not null)
        {
            query = query.OrderBy(specification.OrderBy);
        }
        else if (specification.OrderByDescending is not null)
        {
            query = query.OrderByDescending(specification.OrderByDescending);
        }
        if (specification.IsPagingEnabled)
        {
            query = query
                .Skip(specification.Skip!.Value)
                .Take(specification.Take!.Value);
        }
        return query;
    }
}
