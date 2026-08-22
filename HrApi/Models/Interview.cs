namespace HrApi.Models;

public sealed class Interview
{
    public int Id { get; set; }

    public int CandidateId { get; set; }

    public Candidate Candidate { get; set; } = null!;

    public DateTime ScheduledAtUtc { get; set; }

    public string InterviewType { get; set; } = string.Empty;

    public string InterviewerName { get; set; } = string.Empty;

    public string? Notes { get; set; }

    public int? Score { get; set; }

    public DateTime CreatedAtUtc { get; set; }
}
