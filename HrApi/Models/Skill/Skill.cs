using System;

namespace HrApi.Models.Skill;

public class Skill
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }

    public ICollection<PositionSkill> Positions { get; set;}
        = new List<PositionSkill>();

    public ICollection<EmployeeSkillState> EmployeeSkillStates { get; set; }
        = new List<EmployeeSkillState>();
}
