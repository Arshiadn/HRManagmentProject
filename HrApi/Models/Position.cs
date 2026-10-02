using System;
using HrApi.Models.Skill;

namespace HrApi.Models;

public class Position
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public ICollection<Employee> Employees { get; set; }
        = new List<Employee>();

    public ICollection<PositionSkill> Skills { get; set;}
        = new List<PositionSkill>();
}
