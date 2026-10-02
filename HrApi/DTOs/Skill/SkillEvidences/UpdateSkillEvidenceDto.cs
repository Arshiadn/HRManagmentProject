using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.SkillEvidences;

public sealed class UpdateSkillEvidenceDto
{
    public SkillEvidenceType Type { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }

    public string Reason { get; set; } = string.Empty;
}
