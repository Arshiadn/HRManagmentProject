using System;

namespace HrApi.DTOs.Skill.History;

public sealed class EmployeeSkillHistoryDto
{
    public int SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public List<SkillStateHistoryDto> History { get; set; } = new();
}
