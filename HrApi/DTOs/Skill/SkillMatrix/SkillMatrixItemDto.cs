using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.SkillMatrix;

public sealed class SkillMatrixItemDto
{
    public int SkillId { get; set; } 
    public string SkillName { get; set; } = string.Empty; 
    public SkillLevel RequiredLevel { get; set; } 
    public SkillLevel CurrentLevel { get; set; } 
    public SkillMatrixStatus Status { get; set; }
}
