using HrApi.Enums.Asset;

namespace HrApi.DTOs.Assets;

public sealed class EmployeeAssetDto
{
    public int AssetId { get; init; }

    public DateTimeOffset AssignedAt { get; init; }
}
