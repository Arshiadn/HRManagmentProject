using HrApi.Enums;

namespace HrApi.DTOs.Candidates;

public sealed class CandidateListRequest
{
    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = 20;

    public string? Search { get; set; }

    public RecruitmentStage? Stage { get; set; }

    public DateTime? CreatedAtUtc { get; set; }
}
