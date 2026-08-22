using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Candidates;

public sealed class RejectCandidateRequest
{
    [Required]
    [MaxLength(1000)]
    public string Reason { get; set; } = string.Empty;
}
