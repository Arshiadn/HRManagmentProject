using HrApi.ValueObjects;

namespace HrApi.Models;

public sealed class Shift
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public TimeRange WorkingHours { get; set; }
    public int GraceMinutes { get; set; }
    public bool IsActive { get; set; }
    public ICollection<EmployeeShiftAssignment> EmployeeAssignments { get; set; }
         = new List<EmployeeShiftAssignment>();
}
