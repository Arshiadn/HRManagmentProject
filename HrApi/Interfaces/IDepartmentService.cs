using HrApi.Data;
using HrApi.DTOs.Departments;
using HrApi.Models;

namespace HrApi.Interfaces;

public interface IDepartmentService
{
    Task<IReadOnlyList<DepartmentListDto>>
        GetAllAsync(CancellationToken cancellationToken);

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
}
