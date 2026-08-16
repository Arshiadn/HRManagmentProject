namespace HrApi.DTOs.Employees;

public class EmployeeListDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PersonnelCode { get; set; }
    public string Email { get; set; }
    public string DepartmentName { get; set; }
    public bool IsActive { get; set; }
}
