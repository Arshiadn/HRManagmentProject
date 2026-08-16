namespace HrApi.DTOs.Employees;

public class EmployeeDetailsDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string DepartmentName { get; set; }
    public decimal Salary { get; set; }
}
