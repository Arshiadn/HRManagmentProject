namespace HrApi.DTOs.Employees;

// Hr manager will have this job assigned later
public class UpdateEmployeeDto
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public decimal Salary { get; set; }
    public string PersonnelCode { get; set; }
    public bool IsActive { get; set; }
}
