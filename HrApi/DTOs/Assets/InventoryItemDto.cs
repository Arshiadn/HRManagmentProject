using HrApi.Enums.Asset;

namespace HrApi.DTOs.Assets;

public sealed class InventoryItemDto
{
    public AssetStatus Status { get; init; }

    public int Count { get; init; }
}
