using System.Diagnostics.Contracts;
using HrApi.Models.Skill;

namespace HrApi.Models;

public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PersonnelCode { get; set; } = string.Empty;
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

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;
    
    public ICollection<EmployeeContract> Contracts { get; set; } 
        = new List<EmployeeContract>();
    public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        = new List<AttendanceRecord>();
    public ICollection<EmployeeShiftAssignment> ShiftAssignments { get; set; }
        = new List<EmployeeShiftAssignment>();
    public ICollection<EmployeeRequest> Requests { get; set; }
        = new List<EmployeeRequest>();

    public ICollection<EmployeeSkillState> SkillStates { get; set; }
        = new List<EmployeeSkillState>();
}
