using HrApi.Data;
using HrApi.Interfaces;
using HrApi.Specifications;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Repositories;

public sealed class EfRepository<T>
    : IReadRepository<T>
    where T : class
{
    private readonly HrDbContext _context;

    public EfRepository(HrDbContext context) => _context = context;

    public async Task<int> CountAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken)
    {
        var query = _context.Set<T>()
          .AsNoTracking();

        if(specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }

        return await query.CountAsync(cancellationToken);
    }

    public async Task<T?> FirstOrDefaultAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken)
    {
        var query = SpecificationEvaluator.GetQuery(
            _context.Set<T>(),
            specification);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<T>> ListAsync(
        ISpecification<T> specification,
        CancellationToken cancellationToken)
    {
        var query = SpecificationEvaluator.GetQuery(
            _context.Set<T>(),
            specification);

        return await query.ToListAsync(cancellationToken);
    }
}
