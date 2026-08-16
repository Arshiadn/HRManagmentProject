namespace HrApi.Data.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HrApi.DTOs.Departments;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

public class DepartmentService : IDepartmentService
{
    private readonly HrDbContext _context;
    private readonly IMapper _mapper;
    public DepartmentService(HrDbContext context, IMapper mapper)
    {
        _context = context; _mapper = mapper;
    }
    public async Task<IReadOnlyList<DepartmentListDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Departments
                .AsNoTracking()
                .OrderBy(d => d.Name)
                .Select(d => new DepartmentListDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    IsActive = d.IsActive,
                    EmployeeCount = d.Employees.Count()
                })
                .ToListAsync(cancellationToken);       
    }
    public async Task<DepartmentDetailsDto> GetByIdAsync(
            int id,
            CancellationToken cancellationToken)
    {
        var department = await _context.Departments
                            .AsNoTracking()
                            .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if(department is null)
        {
            throw new NotFoundException($"Department with id {id} was not found.");
        }
        
        return _mapper.Map<DepartmentDetailsDto>(department); 
    }
    //dont need this for now(maybe later? or delete)

    //public async Task<Department> GetActiveDepartmentAsync(int departmentId,
    //    CancellationToken cancellationToken)
    //{
    //    var department = await _context.Departments
    //        .FirstOrDefaultAsync(
    //            x => x.Id == departmentId && x.IsActive,
    //                cancellationToken);

    //    if (department is null)
    //    {
    //        throw new BadRequestException(
    //            "The selected department is invalid or inactive.");
    //    }

    //    return department;
    //}
    public async Task<int> CreateAsync(
            CreateDepartmentDto request,
            CancellationToken cancellationToken)
    {
        var normalizedName = request.Name.Trim();
        var exists = await _context.Departments
                    .AnyAsync(d => d.Name == normalizedName, cancellationToken);
        if(exists)
        {
            throw new ConflictException("A department with this name already exists.");
        }

        var department = new Department
        {
            Name = normalizedName,
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Departments.Add(department);

        await _context.SaveChangesAsync(cancellationToken);

        return department.Id;
    }
    public async Task UpdateAsync(
    int id,
    UpdateDepartmentDto request,
    CancellationToken cancellationToken)
    {
        var department = await _context.Departments
                        .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if(department is null)
        {
            throw new NotFoundException($"Department with id {id} was not found.");
        }
        var normalizedName = request.Name.Trim();
        var duplicateExists = await _context.Departments
                              .AnyAsync(d => d.Id != id &&
                                    d.Name == normalizedName,
                                    cancellationToken);
        if(duplicateExists)
        {
            throw new ConflictException("Another department with this name already exists.");
        }
        department.Name = normalizedName;
        department.Description = request.Description?.Trim();
        department.IsActive = request.IsActive;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task SoftDeleteAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var department = await _context.Departments
                        .FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
        if(department is null)
        {
            throw new NotFoundException(
            $"Department with id {id} was not found.");
        }

        var hasEmployee = await _context.Employees
            .AnyAsync(e => e.DepartmentId == id, cancellationToken);
        if(hasEmployee)
        {
            throw new ConflictException(
                "A department with employees cannot be deleted.");
        }
        department.IsDeleted = true;
        department.IsActive = false;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task RestoreAsync(
        int id,
        CancellationToken cancellationToken)
    {
        var department = await _context.Departments
    .IgnoreQueryFilters()
    .FirstOrDefaultAsync(
        d => d.Id == id && d.IsDeleted,
        cancellationToken);

        if (department is null)
        {
            throw new NotFoundException(
                "Deleted department was not found.");
        }

        department.IsDeleted = false;
        department.IsActive = true;
        department.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
