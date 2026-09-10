using HrApi.Enums.Asset;

namespace HrApi.DTOs.Assets;

public sealed class AssetListDto
{
    public int Id { get; init; }

    public string AssetCode { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public AssetType Type { get; init; }

    public AssetStatus Status { get; init; }
}
