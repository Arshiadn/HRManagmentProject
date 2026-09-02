using HrApi.DTOs.ValueObjects;

namespace HrApi.DTOs.Shifts;

public sealed class CreateShiftDto
{
    public string Name { get; set; } = string.Empty;
    public TimeRangeDto WorkingHours { get; set; } = new();
    public int GraceMinutes { get; set; }
    public bool IsActive { get; set; }
}
