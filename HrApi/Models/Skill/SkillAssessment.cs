using System;
using HrApi.Enums.Skill;

namespace HrApi.Models.Skill;

public class SkillAssessment
{
    public long Id { get; set; }

    public long SkillClaimId { get; set; }
    public EmployeeSkillClaim SkillClaim { get; set; } = null!;

    public SkillLevel Decision { get; set; }

    public string AssessorId { get; set; } = null!;
    public DateTime AssessedAt { get; set; } = DateTime.UtcNow;

    public string? Comment { get; set; }
}
