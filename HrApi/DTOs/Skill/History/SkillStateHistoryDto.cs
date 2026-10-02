using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.History;

public sealed class SkillStateHistoryDto
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public int SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public SkillLevel? FromLevel { get; set; }

    public SkillLevel ToLevel { get; set; }

    public long? SkillAssessmentId { get; set; }

    public string Reason { get; set; } = string.Empty;

    public DateTime OccurredAt { get; set; }
}
