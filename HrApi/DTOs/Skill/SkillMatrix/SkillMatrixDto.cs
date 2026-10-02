using System;

namespace HrApi.DTOs.Skill.SkillMatrix;

public sealed class SkillMatrixDto
{
    public int EmployeeId { get; set; } 
    public string EmployeeName { get; set; } = string.Empty;
    public int PositionId { get; set; }
    public string PositionTitle { get; set; } = string.Empty;

    public List<SkillMatrixItemDto> Skills { get; set; } = new();
}
