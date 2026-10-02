using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.PositionSkills;

public sealed class UpdatePositionSkillDto
{
    public SkillLevel RequiredLevel { get; set; }
    public string Reason { get; set; } = string.Empty;
}
