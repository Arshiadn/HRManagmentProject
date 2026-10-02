using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.EmployeeSkillClaim;

public sealed class EmployeeSkillClaimDto
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public int SkillId { get; set; }

    public string SkillName { get; set; } = string.Empty;

    public SkillLevel ClaimedLevel { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<long> EvidenceIds { get; set; } = new();

    public bool IsActive { get; set; }
}
