using HrApi.Enums;
using HrApi.DTOs.Interviews;
using HrApi.DTOs.StageHistory;

namespace HrApi.DTOs.Candidates;

public sealed class CandidateDetailsDto
{
    public int Id { get; init; }

    public string FullName { get; init; } = string.Empty;

    public string Email { get; init; } = string.Empty;

    public string PhoneNumber { get; init; } = string.Empty;

    public RecruitmentStage Stage { get; init; }

    public string? RejectionReason { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public IReadOnlyList<InterviewDto> Interviews { get; init; }
        = Array.Empty<InterviewDto>();

    public IReadOnlyList<StageHistoryDto> StageHistory { get; init; }
        = Array.Empty<StageHistoryDto>();
}
