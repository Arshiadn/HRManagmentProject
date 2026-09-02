using HrApi.DTOs.ValueObjects;

namespace HrApi.DTOs.ShiftAssignment;

public sealed class ShiftAssignmentDetailsDto
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public int ShiftId { get; set; }

    public string ShiftName { get; set; } = string.Empty;

    public TimeRangeDto WorkingHours { get; set; } = new();

    public int GraceMinutes { get; set; }

    public DateOnly EffectiveFrom { get; set; }

    public DateOnly? EffectiveTo { get; set; }
}
