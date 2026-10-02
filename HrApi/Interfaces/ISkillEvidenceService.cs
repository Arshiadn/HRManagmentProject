using System;
using HrApi.DTOs.Skill.SkillEvidences;

namespace HrApi.Interfaces;

public interface ISkillEvidenceService
{
    Task<SkillEvidenceDto> CreateAsync(
        CreateSkillEvidenceDto dto,
        CancellationToken cancellationToken);

    Task<List<SkillEvidenceDto>> GetMineAsync(
        CancellationToken cancellationToken);


    Task<List<SkillEvidenceDto>> GetByEmployeeIdAsync(
        int employeeId,
        CancellationToken cancellationToken);

    Task<SkillEvidenceDto> UpdateAsync(
        long evidenceId,
        UpdateSkillEvidenceDto dto,
        CancellationToken cancellationToken);
}
