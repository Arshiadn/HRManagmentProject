using HrApi.Enums.Asset;
using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Assets;

public sealed class CreateAssetDto
{
    [Required]
    public string AssetCode { get; set; } = string.Empty;
    [Required]
    public string Title { get; set; } = string.Empty;

    public string? SerialNumber { get; set; }

    public AssetType Type { get; set; }
}
