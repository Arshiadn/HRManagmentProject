namespace HrApi.Data.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using HrApi.DTOs.Departments;
using HrApi.Interfaces;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;

public class DepartmentService : IDepartmentService
{
    private readonly HrDbContext _context;
    private readonly IMapper _mapper;
    public DepartmentService(HrDbContext context, IMapper mapper)
    {
        _context = context; _mapper = mapper;
    }
    public List<DepartmentListDto> GetAll()
    {
        return _context.Departments
            .AsNoTracking()
            .OrderBy(d => d.Name)
            .ProjectTo<DepartmentListDto>(_mapper.ConfigurationProvider)
            .ToList();
    }
    public DepartmentDetailsDto? GetById(int id)
    {
        return _context.Departments
            .AsNoTracking()
            .Where(d => d.Id == id)
            .ProjectTo<DepartmentDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefault();
    }
    public DepartmentDetailsDto Create(CreateDepartmentDto model)
    {
        var nameExists = _context.Departments.Any(d =>  d.Name == model.Name);
        if (nameExists)
        {
            throw new InvalidOperationException("این دپارتمان ثبت شده است");
        }
        var department = _mapper.Map<Department>(model);
        _context.Departments.Add(department);
        _context.SaveChanges();

        return _mapper.Map<DepartmentDetailsDto>(department);
    }
    public void Update(int id, UpdateDepartmentDto model)
    {
        var department = _context.Departments.FirstOrDefault(d => d.Id == id);
        if (department == null)
        {
            throw new InvalidOperationException("دپارتمان مورد نظر پیدا نشد");
        }
        _mapper.Map(model, department);
        _context.SaveChanges();
    }
    public void Delete(int id)
    {
        var department = _context.Departments.FirstOrDefault(d => d.Id == id);
        if (department == null)
        {
            throw new InvalidOperationException("دپارتمان مورد نظر پیدا نشد");
        }
        _context.Departments.Remove(department);
        _context.SaveChanges();
    }
}
