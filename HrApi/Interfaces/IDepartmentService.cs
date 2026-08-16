using HrApi.Data;
using HrApi.DTOs.Departments;

namespace HrApi.Interfaces;

public interface IDepartmentService
{
    List<DepartmentListDto> GetAll();
    DepartmentDetailsDto? GetById(int id);
    DepartmentDetailsDto Create(CreateDepartmentDto model);
    void Update(int id, UpdateDepartmentDto model);
    void Delete(int id);
}
