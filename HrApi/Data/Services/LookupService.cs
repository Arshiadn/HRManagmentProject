namespace HrApi.Data.Services;

using AspNetCoreGeneratedDocument;
using HrApi.DTOs.Lookup;
using HrApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class LookupService : ILookupService
{
    private readonly HrDbContext _context;
    public LookupService(HrDbContext context) => _context = context;

    public async Task<IReadOnlyList<LookupItemDto>>
        GetActiveDepartmentsAsync(CancellationToken cancellationToken)
    {
        return await _context.Departments
            .AsNoTracking()
            .Where(d => d.IsActive)
            .OrderBy(d => d.Name)
            .Select(d => new LookupItemDto
            {
                Id =d.Id,
                Title = d.Name
            })
            .ToListAsync(cancellationToken);
    }
}
