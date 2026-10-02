using System;
using HrApi.Enums.Skill;

namespace HrApi.Models.Skill;

public class SkillEvidence
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int SkillId { get; set; }
    public Skill Skill { get; set; } = null!;

    public SkillEvidenceType Type { get; set; }
    public DateTime IssuedAt { get; set; }
    public DateTime? ExpiresAt { get; set; }

    public ICollection<EmployeeSkillClaim> Claims { get; set; }
        = new List<EmployeeSkillClaim>();
    public ICollection<SkillEvidenceHistory> History { get; set; }
        = new List<SkillEvidenceHistory>();
}
