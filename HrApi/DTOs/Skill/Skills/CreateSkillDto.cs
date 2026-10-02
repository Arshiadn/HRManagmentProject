using System;

namespace HrApi.DTOs.Skill.Skills;

public sealed class CreateSkillDto
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}
