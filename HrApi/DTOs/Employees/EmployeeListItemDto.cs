namespace HrApi.DTOs.Employees;

public sealed class EmployeeListItemDto
{
    public int Id { get; set; }
    public string PersonnelCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public int DepartmentId { get; set; }
    public bool IsActive { get; set; }
}
