using HrApi.Enums;

namespace HrApi.Models;

public sealed class Candidate
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? ResumeFileName { get; set; }
    public string? ResumeStoredName { get; set; }
    public RecruitmentStage Stage { get; set; }
    public string? RejectionReason { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? UpdatedAtUtc { get; set; }
    public DateTime? HiredAtUtc { get; set; }
    public int? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
    public ICollection<Interview> Interviews { get; set; }
        = new List<Interview>();
    public ICollection<RecruitmentStageHistory> StageHistory { get; set; } 
        = new List<RecruitmentStageHistory>();
}
