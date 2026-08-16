using AutoMapper;
using AutoMapper.QueryableExtensions;
using Azure.Core;
using HrApi.DTOs.Employees;
using HrApi.DTOs.Paging;
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
            throw new InvalidOperationException("این ایمیل قبلاً ثبت شده است");
        }
        var employee = _mapper.Map<Employee>(model);
        employee.DepartmentName = "Unknown";
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
            throw new InvalidOperationException("کارمند مورد نظر پیدا نشد");
        }
        _mapper.Map(model, employee);
        employee.DepartmentName = "Unknown";
        _context.SaveChanges();
    }
    public void Delete(int id)
    {
        var employee = _context.Employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
            throw new InvalidOperationException("کارمند مورد نظر پیدا نشد");
        _context.Employees.Remove(employee);
        _context.SaveChanges();
    }
    public async Task UploadProfileImageAsync(int id, string imagePath)
    {
        var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (employee == null)
        {
            throw new InvalidOperationException("کارمند مورد نظر پیدا نشد");
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
            throw new InvalidOperationException("کارمند مورد نظر پیدا نشد");
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
        if(employee == null)
        {
            throw new InvalidOperationException($"کارمند با آیدی {id} پیدا نشد");
        }
        if (string.IsNullOrWhiteSpace(employee.PhotoPath))
        {
            throw new InvalidOperationException("تصویری ثبت نشده است");
        }
        var request = _httpContextAccessor.HttpContext?.Request;

        var fullUrl = $"{request?.Scheme}://{request.Host}{employee.PhotoPath}";

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
            throw new InvalidOperationException($"کارمند با آیدی {id} پیدا نشد");
        }

        if (string.IsNullOrWhiteSpace(employee.ContractPath))
        {
            throw new InvalidOperationException("تصویری ثبت نشده است");
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
            throw new InvalidOperationException($"کارمند با آیدی {id} پیدا نشد");
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
}
