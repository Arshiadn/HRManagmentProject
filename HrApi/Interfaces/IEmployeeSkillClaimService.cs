using System;
using HrApi.DTOs.Skill.EmployeeSkillClaim;

namespace HrApi.Interfaces;

public interface IEmployeeSkillClaimService
{
    Task<EmployeeSkillClaimDto> CreateAsync(
    CreateEmployeeSkillClaimDto dto,
    CancellationToken cancellationToken);

    Task<List<EmployeeSkillClaimDto>> GetByEmployeeIdAsync(
    int employeeId,
    CancellationToken cancellationToken);
}
