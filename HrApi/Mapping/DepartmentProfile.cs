using AutoMapper;
using HrApi.DTOs.Departments;
using HrApi.Models;

namespace HrApi.Mapping;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        CreateMap<Department, DepartmentListDto>();
        CreateMap<Department, DepartmentDetailsDto>();
        CreateMap<CreateDepartmentDto, Department>();
        CreateMap<UpdateDepartmentDto, Department>();
    }
}
