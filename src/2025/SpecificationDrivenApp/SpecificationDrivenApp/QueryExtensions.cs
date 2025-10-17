using Microsoft.EntityFrameworkCore;

namespace SpecificationDrivenApp;

public static class QueryExtensions
{
    public static IQueryable<T> Specify<T>(this IQueryable<T> query, ISpecification<T> specification) where T : class
    {
        var queryableWithIncludes = specification.Includes
            .Aggregate(query, (current, include) => current.Include(include));

        return queryableWithIncludes.Where(specification.Criteria);
    }
}