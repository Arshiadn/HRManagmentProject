using System;
using HrApi.Enums.Skill;

namespace HrApi.Models.Skill;

public class EmployeeSkillClaim
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;

    public SkillLevel ClaimedLevel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsActive { get; set; } = true;

    public ICollection<SkillEvidence> Evidence { get; set; }
        = new List<SkillEvidence>();
}
