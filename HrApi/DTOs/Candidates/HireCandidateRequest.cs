using System.ComponentModel.DataAnnotations;

namespace HrApi.DTOs.Candidates;

public sealed class HireCandidateRequest
{
    [Required]
    public int DepartmentId { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    [MaxLength(30)]
    public string PersonnelCode { get; set; } = string.Empty;
}
