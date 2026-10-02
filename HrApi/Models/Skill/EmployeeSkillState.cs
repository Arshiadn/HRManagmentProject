using System;
using HrApi.Enums.Skill;

namespace HrApi.Models.Skill;

public class EmployeeSkillState
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;

    public SkillLevel CurrentLevel { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
