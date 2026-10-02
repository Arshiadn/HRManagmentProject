using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Positions;

public sealed class PositionSkillHistoryDto
{
    public long Id { get; set; }

    public int PositionSkillId { get; set; }

    public SkillLevel OldRequiredLevel { get; set; }

    public SkillLevel NewRequiredLevel { get; set; }

    public string Reason { get; set; } = string.Empty;

    public string ChangedByUserId { get; set; } = null!;

    public DateTime ChangedAt { get; set; }
}
