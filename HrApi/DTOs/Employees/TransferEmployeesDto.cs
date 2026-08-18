namespace HrApi.DTOs.Employees;

public class TransferEmployeesDto
{
    public int TargetDepartmentId { get; set; }
    public List<int> EmployeeIds { get; set; } = new();
}
