using System;
using HrApi.Enums.Skill;
using HrApi.Models.Skill;

namespace HrApi.Models.Skill;

public class PositionSkillHistory
{
    public long Id { get; set; }

    public int PositionSkillId { get; set; }
    public PositionSkill PositionSkill { get; set; } = null!;

    public SkillLevel OldRequiredLevel { get; set; }

    public SkillLevel NewRequiredLevel { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string ChangedByUserId { get; set; } = null!;

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
