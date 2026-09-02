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
    public DateTime HireDateFrom { get; set; }
    public DateTime? HireDateTo { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;
    public ICollection<EmployeeContract> Contracts { get; set; } 
        = new List<EmployeeContract>();
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new List<AttendanceRecord>();
    public ICollection<EmployeeShiftAssignment> ShiftAssignments { get; set; }
        = new List<EmployeeShiftAssignment>();
}
