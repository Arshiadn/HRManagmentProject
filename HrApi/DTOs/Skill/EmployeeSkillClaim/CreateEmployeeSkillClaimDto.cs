using System;
using HrApi.Enums.Skill;

namespace HrApi.DTOs.Skill.EmployeeSkillClaim;

public sealed class CreateEmployeeSkillClaimDto
{
    public int SkillId { get; set; }

    public SkillLevel ClaimedLevel { get; set; }

    public List<long> EvidenceIds { get; set; } = new();
}
