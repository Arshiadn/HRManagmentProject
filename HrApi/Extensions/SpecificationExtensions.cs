using System.Linq.Expressions;

namespace HrApi.Extensions;

public static class SpecificationExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
        where T : class
    {
        var parameter = Expression.Parameter(typeof(T));

        var body = Expression.AndAlso(
            Expression.Invoke(left, parameter),
            Expression.Invoke(right, parameter));

        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}
