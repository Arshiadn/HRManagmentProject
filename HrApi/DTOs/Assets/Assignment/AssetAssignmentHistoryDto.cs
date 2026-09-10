namespace HrApi.DTOs.Assets.Assignment;

public sealed class AssetAssignmentHistoryDto
{
    public long AssignmentId { get; init; }

    public int AssetId { get; set; }

    public int EmployeeId { get; init; }

    public DateTimeOffset AssignedAt { get; init; }

    public DateTimeOffset? ReturnedAt { get; init; }

    public string? AssignmentNote { get; init; }

    public string? ReturnNote { get; init; }
}
