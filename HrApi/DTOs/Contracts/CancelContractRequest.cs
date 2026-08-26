using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Contracts;

public sealed class CancelContractRequest // Cancel
{
    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;
}
