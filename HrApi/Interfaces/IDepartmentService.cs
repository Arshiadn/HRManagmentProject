using HrApi.Data;
using HrApi.DTOs.Departments;
using HrApi.DTOs.Employees;
using HrApi.Models;

namespace HrApi.Interfaces;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentListDto>>
        GetAllAsync(CancellationToken cancellationToken);

    Task<IReadOnlyList<DepartmentListDto>>
        GetDeletedListAsync(CancellationToken cancellationToken);

    Task<DepartmentDetailsDto>
        GetByIdAsync(
            int id,
            CancellationToken cancellationToken);

    //Task<Department> GetActiveDepartmentAsync(
    //    int departmentId,
    //    CancellationToken cancellationToken);

    Task<int>
        CreateAsync(
            CreateDepartmentDto request,
            CancellationToken cancellationToken);

    Task UpdateAsync(
        int id,
        UpdateDepartmentDto request,
        CancellationToken cancellationToken);

    Task SoftDeleteAsync(
        int id,
        CancellationToken cancellationToken);

    Task RestoreAsync(
        int id,
        CancellationToken cancellationToken);

    Task<List<EmployeeListDto>> GetEmployeesAsync(
        int id,
        CancellationToken cancellationToken);
}
