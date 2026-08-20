using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using HrApi.DTOs.Employees;
using HrApi.DTOs.Paging;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace HrApi.Data.Services;

public class EmployeeService : IEmployeeService
{
    private readonly HrDbContext _context;
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorage;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public EmployeeService(HrDbContext context,
        IMapper mapper,
        IFileStorageService fileStorage,
        IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _httpContextAccessor = httpContextAccessor;
    }
    public List<EmployeeListDto> GetAll()
    {
        return _context.Employees
            .AsNoTracking()
            .OrderBy(e => e.FullName)
            .ProjectTo<EmployeeListDto>(_mapper.ConfigurationProvider)
            .ToList();
    }
    public EmployeeDetailsDto? GetById(int id)
    {
        return _context.Employees
            .Where(e => e.Id == id)
            .AsNoTracking()
            .ProjectTo<EmployeeDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();
    }
    public EmployeeDetailsDto Create(CreateEmployeeDto model)
    {
        var emailExists = _context.Employees
            .Any(e => e.Email == model.Email);
        if (emailExists)
        {
            throw new ConflictException("Email already exists");
        }
        var employee = _mapper.Map<Employee>(model);
        employee.IsActive = true;

        _context.Employees.Add(employee);
        _context.SaveChanges();

        return _mapper.Map<EmployeeDetailsDto>(employee);
    }
    public void Update(int id, UpdateEmployeeDto model)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }
        _mapper.Map(model, employee);

        _context.SaveChanges();
    }
    public void Delete(int id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }  
        _context.Employees.Remove(employee);
        _context.SaveChanges();
    }
    public async Task UploadProfileImageAsync(int id, string imagePath)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }
        employee.ProfileImagePath = imagePath;
        await _context.SaveChangesAsync();
    }
    public async Task<PagedResultDto<EmployeeListDto>> Search([FromQuery] EmployeeSearchRequestDto request)
    {
        var query = _context.Employees
           .AsNoTracking()
           .Include(x => x.Department)
           .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Term))
        {
            var term = request.Term.Trim().ToLower();

            query = query.Where(x =>
                x.FullName.ToLower().Contains(term) ||
                x.Email.ToLower().Contains(term) ||
                x.PersonnelCode.ToLower().Contains(term)
            );
        }

        if (request.DepartmentId.HasValue)
        {
            query = query.Where(x =>
                x.DepartmentId == request.DepartmentId.Value
            );
        }

        if (request.IsActive.HasValue)
        {
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value
            );
        }

        query = query.OrderBy(x => x.FullName);

        var page = request.Page <= 0 ? 1 : request.Page;

        var pageSize = request.PageSize <= 0
            ? 10
            : request.PageSize;

        if (pageSize > 100)
        {
            pageSize = 100;
        }

        var totalCount = await query.CountAsync();

        var employees = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = _mapper.Map<List<EmployeeListDto>>(employees);

        return new PagedResultDto<EmployeeListDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)pageSize)
        };
    }
    public async Task<EmployeePhotoDto> UploadPhotoAsync(int id, [FromForm] EmployeePhotoUploadDto model)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }

        var newPath = await _fileStorage.SavePublicImageAsync(model.Photo, "employees");

        if (!string.IsNullOrWhiteSpace(newPath))
        {
            await _fileStorage.DeletePublicFileAsync(employee.PhotoPath);
        }
        employee.PhotoPath = newPath;

        await _context.SaveChangesAsync();

        return new EmployeePhotoDto
        {
            EmployeeId = id,
            PhotoUrl = newPath
        };
    }
    public async Task<EmployeePhotoDto> GetPhotoAsync(int id)
    {
        var employee = await _context.Employees
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }
        if (string.IsNullOrWhiteSpace(employee.PhotoPath))
        {
            throw new BadRequestException("No photo submited");
        }
        var request = _httpContextAccessor.HttpContext?.Request;

        var fullUrl = $"{request?.Scheme}://{request?.Host}{employee.PhotoPath}";

        return new EmployeePhotoDto
        {
            EmployeeId = id,
            PhotoUrl = fullUrl
        };
    }
    public async Task<StoredFileResult?> DownloadContractAsync(int id)
    {
        var employee = await _context.Employees
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }

        if (string.IsNullOrWhiteSpace(employee.ContractPath))
        {
            throw new BadRequestException("No photo submited");
        }

        return await _fileStorage.GetPrivateFileAsync(
            employee.ContractPath
        );
    }
    public async Task DeletePhotoAsync(int id)
    {
        var employee = await _context.Employees
       .FirstOrDefaultAsync(x => x.Id == id);

        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }

        if (!string.IsNullOrWhiteSpace(employee.PhotoPath))
        {
            await _fileStorage.DeletePublicFileAsync(
                employee.PhotoPath
            );

            employee.PhotoPath = null;

            await _context.SaveChangesAsync();
        }
    }
    public async Task AssignPersonnelCodeAsync(
    int id,
    string personnelCode,
    CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (employee == null)
        {
            throw new NotFoundException($"Employee {id} not found.");
        }

        employee.PersonnelCode = personnelCode;

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task TransferEmployeesAsync
        (TransferEmployeesDto request, CancellationToken cancellationToken)
    {
        var departmentExists = await _context.Departments
            .AnyAsync(d => d.Id == request.TargetDepartmentId, cancellationToken);
        if (!departmentExists)
        {
            throw new NotFoundException("Target department not found.");
        }
        var employees = await
                _context.Employees
                    .Where(e => request.EmployeeIds.Contains(e.Id))
                    .ToListAsync(cancellationToken);
        if (employees.Count != request.EmployeeIds.Count)
        {
            throw new NotFoundException("One or more employees were not found");
        }
        foreach(var employee in employees)
        {
            employee.DepartmentId = request.TargetDepartmentId;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<PagedResultDto<EmployeeListItemDto>>
        GetListAsync(EmployeeListRequest request,
            CancellationToken cancellationToken)
    {
        var query = _context.Employees
                        .AsNoTracking()
                        .AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(s =>
            s.FullName.Contains(search) ||
            s.Email.Contains(search) ||
            s.PersonnelCode.Contains(search));
        }
        if (request.DepartmentId.HasValue)
            query = query.Where(x =>
                x.DepartmentId == request.DepartmentId.Value);

        if (request.IsActive.HasValue)
            query = query.Where(x =>
                x.IsActive == request.IsActive.Value);

        var desc = string.Equals(
            request.SortDirection,
            "desc",
            StringComparison.OrdinalIgnoreCase);

        var sortBy = request.SortBy?.ToLowerInvariant() ?? "fullname";

        query = sortBy switch
        {
            "email" => desc
                ? query.OrderByDescending(x => x.Email)
                    .ThenBy(x => x.Id)
                : query.OrderBy(x => x.Email)
                    .ThenBy(x => x.Id),
            "personnelcode" => desc
                ? query.OrderByDescending(x => x.PersonnelCode)
                    .ThenBy(x => x.Id)
                : query.OrderBy(x => x.PersonnelCode)
                    .ThenBy(x => x.Id),
            "fullname" => desc
                ? query.OrderByDescending(x => x.FullName)
                    .ThenBy(x => x.Id)
                : query.OrderBy(x => x.FullName)
                    .ThenBy(x => x.Id),
            _ => throw new BadRequestException("Invalid SortBy input")
        };

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new EmployeeListItemDto
            {
                Id = x.Id,
                PersonnelCode = x.PersonnelCode,
                FullName = x.FullName,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.Name,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);

        return new PagedResultDto<EmployeeListItemDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalItems,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)request.PageSize)
        };
    }
}
