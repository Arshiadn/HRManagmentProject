using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.SkillAssessments;

public sealed class SkillAssessmentDto
{
 public long Id { get; set; }

    public long SkillClaimId { get; set; }

    public int EmployeeId { get; set; }

    public int SkillId { get; set; }

    public SkillLevel Decision { get; set; }

    public string AssessorId { get; set; } = null!;

    public DateTime AssessedAt { get; set; }

    public string? Comment { get; set; }
}
