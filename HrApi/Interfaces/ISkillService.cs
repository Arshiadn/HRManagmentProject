using System;
using HrApi.DTOs.Skill.Skills;
using HrApi.Models.Skill;

namespace HrApi.Interfaces;

public interface ISkillService
{
    Task<SkillDetailsDto> CreateAsync(
        CreateSkillDto dto,
        CancellationToken cancellationToken);

    Task<List<SkillDetailsDto>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<SkillDetailsDto> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task<SkillDetailsDto> UpdateAsync(
        int id,
        UpdateSkillDto dto,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        int id,
        CancellationToken cancellationToken);
}
