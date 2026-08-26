using HrApi.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Linq.Expressions;

namespace HrApi.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    protected BaseSpecification() { }

    protected BaseSpecification(
        Expression<Func<T , bool>> criteria)
    {
        Criteria = criteria;
    }
    
    public Expression<Func<T, bool>>? Criteria { get; }

    public List<Expression<Func<T, object>>> IncludeExpressions = [];

    public IReadOnlyList<Expression<Func<T, object>>> Includes => IncludeExpressions;

    public Expression<Func<T, object>>? OrderBy { get; private set; }

    public Expression<Func<T, object>>? OrderByDescending { get; private set; }

    public int? Skip {  get; private set; }

    public int? Take { get; private set; }

    public bool IsPagingEnabled { get; private set; }

    public bool AsNoTracking { get; private set; } = true;

    protected void AddInclude(
        Expression<Func<T, object>> include)
    {
        IncludeExpressions.Add(include);
    }
    protected void ApplyOrderBy(
    Expression<Func<T, object>> expression)
    {
        OrderBy = expression;
    }
    protected void ApplyOrderByDescending(
        Expression<Func<T, object>> expression)
    {
        OrderByDescending = expression;
    }
    protected void ApplyPaging(
        int skip, int take)
    {
        Skip = skip;
        Take = take;
        IsPagingEnabled = true;
    }
    protected void UseTracking()
    {
        AsNoTracking = false;
    }
}