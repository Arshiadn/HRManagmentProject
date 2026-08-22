using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Interviews;

public sealed class CompleteInterviewRequest
{
    [Range(0, 100)]
    public int Score { get; set; }

    [MaxLength(2000)]
    public string? Notes { get; set; }
}
