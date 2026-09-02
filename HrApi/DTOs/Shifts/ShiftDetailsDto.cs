using HrApi.DTOs.ValueObjects;

namespace HrApi.DTOs.Shifts;

public sealed class ShiftDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public TimeRangeDto WorkingHours { get; set; }
    public int GraceMinutes { get; set; }
    public bool IsActive { get; set; }
}
