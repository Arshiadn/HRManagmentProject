namespace HrApi.DTOs.Employees;

public class UpdateEmployeeDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public int DepartmentId { get; set; }
    public decimal Salary { get; set; }
    public bool IsActive { get; set; }
}
