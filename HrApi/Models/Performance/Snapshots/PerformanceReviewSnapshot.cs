namespace HrApi.Models.Performance.Snapshots;

public sealed class PerformanceReviewSnapshot
{
    public int RubricVersion { get; init; }
    public List<SnapshotCriterion> Criteria { get; init; } = [];
    public decimal OverallScore { get; init; }
}
