using System;
using HrApi.Enums.Skill;
using HrApi.Models.Skill;

namespace HrApi.Models.Skill;

public class PositionSkill
{
    public int Id { get; set; }

    public int PositionId { get; set; }
    public Position Position { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;
    
    public SkillLevel RequiredLevel { get; set; }

    public ICollection<PositionSkillHistory> History { get; set; }
        = new List<PositionSkillHistory>();
}
