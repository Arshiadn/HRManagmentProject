using HrApi.Enums;
using HrApi.Models;

namespace HrApi.DTOs.StageHistory;

public sealed class StageHistoryDto
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public RecruitmentStage FromStage { get; set; }

    public RecruitmentStage ToStage { get; set; }

    public string? Reason { get; set; }

    public DateTime ChangedAtUtc { get; set; }

    public string ChangedByUserId { get; set; } = string.Empty;
}

