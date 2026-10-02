using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.SkillEvidences;

public sealed class SkillEvidenceDto
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public int SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public SkillEvidenceType Type { get; set; }

    public DateTime IssuedAt { get; set; }

    public DateTime? ExpiresAt { get; set; }
}
