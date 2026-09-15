namespace HrApi.DTOs.Reviews.PerformanceReview;

public sealed class PerformanceReviewHistoryDto
{
    public Guid Id { get; init; }

    public string PeriodTitle { get; init; } = null!;

    public string Status { get; init; } = null!;

    public decimal? OverallScore { get; init; }

    public string? RubricSnapshotJson { get; init; }

    public DateTimeOffset? SubmittedAt { get; init; }

    public DateTimeOffset? AcknowledgedAt { get; init; }
}
