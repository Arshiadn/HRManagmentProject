using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Contracts;

public sealed class CompleteContractRequest
{
    [Required]
    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    [Range(0, 5)]
    public double ScoreRate { get; set; }
}
