using AutoMapper;
using HrApi.DTOs.Employees;
using HrApi.Models;

namespace HrApi.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        //Read
        CreateMap<Employee, EmployeeListDto>()
                .ForMember(d => d.DepartmentName,
                    opt => opt.MapFrom(s => s.Department.Name));
        CreateMap<Employee, EmployeeDetailsDto>()
            .ForMember(d => d.DepartmentName,
                    opt => opt.MapFrom(s => s.Department.Name));
        //Create
        CreateMap<CreateEmployeeDto, Employee>();
        //Update
        CreateMap<UpdateEmployeeDto, Employee>();
    }
}
