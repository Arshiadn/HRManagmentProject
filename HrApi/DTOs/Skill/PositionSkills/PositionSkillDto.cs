using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.PositionSkills;

public sealed class PositionSkillDto
{
    public int Id { get; set; }

    public int PositionId { get; set; }

    public int SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public SkillLevel RequiredLevel { get; set; }
}
