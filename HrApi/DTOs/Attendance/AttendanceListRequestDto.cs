namespace HrApi.DTOs.Attendance;

public sealed class AttendanceListRequestDto
{
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
