using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.SkillEvidences;

public sealed class CreateSkillEvidenceDto
{
    public int SkillId { get; set; }

    public SkillEvidenceType Type { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }
}