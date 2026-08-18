namespace HrApi.DTOs.Employees;

public class EmployeeListDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string PersonnelCode { get; set; }
    public bool IsActive { get; set; }
}
