namespace HrApi.Models;

public sealed class AssetAssignment
{
    public long Id { get; set; }

    public int AssetId { get; set; }
    public CompanyAsset Asset { get; set; } = null!;

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public DateTimeOffset AssignedAt { get; set; }
    public DateTimeOffset? ReturnedAt { get; set; }

    public string? AssignmentNote { get; set; }
    public string? ReturnNote { get; set; }
}
