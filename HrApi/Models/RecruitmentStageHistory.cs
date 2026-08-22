using HrApi.Enums;

namespace HrApi.Models;

public sealed class RecruitmentStageHistory
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public Candidate Candidate { get; set; } = null!;
    
    public RecruitmentStage FromStage { get; set; }

    public RecruitmentStage ToStage { get; set; }

    public string? Reason { get; set; }

    public DateTime ChangedAtUtc { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;
}
