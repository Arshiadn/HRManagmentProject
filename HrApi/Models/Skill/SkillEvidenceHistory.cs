using System;
using HrApi.Enums.Skill;

namespace HrApi.Models.Skill;

public class SkillEvidenceHistory
{
    public long Id { get; set; }
    
    public long SkillEvidenceId { get; set; }
    public SkillEvidence SkillEvidence { get; set; } = null!;

    public DateTime? OldIssuedAt { get; set; }
    public DateTime? NewIssuedAt { get; set; }

    public DateTime? OldExpiresAt { get; set; }
    public DateTime? NewExpiresAt { get; set; }

    public SkillEvidenceType? OldType { get; set; }
    public SkillEvidenceType? NewType { get; set; }

    public string Reason { get; set; } = string.Empty;
    public string ChangedByUserId { get; set; } = null!;
    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
}
