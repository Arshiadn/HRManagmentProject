using System;

namespace HrApi.DTOs.Skill.Skills;

public sealed class SkillDetailsDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}
