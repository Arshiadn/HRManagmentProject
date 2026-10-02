using System;
using HrApi.Enums.Skill;

namespace HrApi.Models.Skill;

public class SkillStateHistory
{
    public long Id { get; set; }

    public Employee Employee { get; set; } = null!;
    public int EmployeeId { get; set; }

    public Skill Skill { get; set; } = null!;
    public int SkillId { get; set; }

    public SkillLevel? FromLevel { get; set; }
    public SkillLevel ToLevel { get; set; }

    public long? SkillAssessmentId { get; set; }
    public SkillAssessment? SkillAssessment { get; set; }

    public string Reason { get; set; } = string.Empty;
    public DateTime OccurredAt { get; set; } = DateTime.UtcNow;
}
