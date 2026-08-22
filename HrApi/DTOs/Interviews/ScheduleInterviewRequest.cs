using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Interviews;

public sealed class ScheduleInterviewRequest
{
    public DateTime ScheduledAtUtc { get; set; }

    [Required]
    [MaxLength(50)]
    public string InterviewType { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    public string InterviewerName { get; set; } = string.Empty;

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
