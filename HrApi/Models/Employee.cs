using System.Diagnostics.Contracts;

namespace HrApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PersonnelCode { get; set; }
    public decimal Salary { get; set; }
    public string? ProfileImagePath { get; set; }
    public string? PhotoPath { get; set; }
    public string? ContractPath { get; set; }
    public bool IsActive { get; set; } = true;

    public int DepartmentId { get; set; }
    public string DepartmentName { get; set; }
    public Department Department { get; set; }
}
