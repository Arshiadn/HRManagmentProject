using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.PositionSkills;

public sealed class CreatePositionSkillDto
{
    public int SkillId { get; set; }

    public SkillLevel RequiredLevel { get; set; }
}
