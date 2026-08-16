namespace HrApi.DTOs.Departments;

public class DepartmentListDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public int EmployeeCount { get; set; }
}
