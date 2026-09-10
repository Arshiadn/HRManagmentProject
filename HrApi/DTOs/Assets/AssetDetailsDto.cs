using HrApi.Enums.Asset;

namespace HrApi.DTOs.Assets;

public sealed class AssetDetailsDto
{
    public int Id { get; init; }

    public string AssetCode { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;

    public string? SerialNumber { get; set; }

    public AssetType Type { get; init; }

    public AssetStatus Status { get; init; }
}
