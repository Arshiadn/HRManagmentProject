using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.SkillAssessments;

public sealed class CreateSkillAssessmentDto
{
    public long SkillClaimId { get; set; }

    public SkillLevel Decision { get; set; }

    public string? Comment { get; set; }
}
