using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Candidates;

public sealed class CreateCandidateRequest
{
    [Required]
    [MaxLength(200)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [MaxLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(11)]
    [MinLength(11)]
    public string PhoneNumber { get; set; } = string.Empty;
}
