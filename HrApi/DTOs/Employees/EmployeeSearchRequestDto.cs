namespace HrApi.DTOs.Employees;

public class EmployeeSearchRequestDto
{
    public string? Term { get; set; }

    public int? DepartmentId { get; set; }

    public bool? IsActive { get; set; }

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}
