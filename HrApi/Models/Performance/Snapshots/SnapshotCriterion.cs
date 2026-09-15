namespace HrApi.Models.Performance.Snapshots;

public sealed class SnapshotCriterion
{
    public string Code { get; init; } = null!;
    public decimal Weight { get; init; }
    public int Score { get; init; }
}
