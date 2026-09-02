namespace HrApi.DTOs.ShiftAssignment;

public sealed class CreateShiftAssignmentDto
{
    public int ShiftId { get; set; }
    public DateOnly EffectiveFrom { get; set; }
    public DateOnly? EffectiveTo { get; set; }
}
