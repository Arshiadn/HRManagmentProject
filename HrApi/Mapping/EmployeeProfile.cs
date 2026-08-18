using AutoMapper;
using HrApi.DTOs.Employees;
using HrApi.Models;

namespace HrApi.Mapping;

public class EmployeeProfile : Profile
{
    public EmployeeProfile()
    {
        //Read
        CreateMap<Employee, EmployeeListDto>();
        CreateMap<Employee, EmployeeDetailsDto>();
        //Create
        CreateMap<CreateEmployeeDto, Employee>();
        //Update
        CreateMap<UpdateEmployeeDto, Employee>();
    }
}
