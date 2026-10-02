using System;
using HrApi.DTOs.Positions;
using HrApi.DTOs.Skill.PositionSkills;

namespace HrApi.Interfaces;

public interface IPositionSkillService
{
    Task<PositionSkillDto> CreateAsync(
        int positionId,
        CreatePositionSkillDto dto,
        CancellationToken cancellationToken);

    Task<List<PositionSkillDto>> GetAllAsync(
        int positionId,
        CancellationToken cancellationToken);

    Task UpdateAsync(
        int positionId,
        int skillId,
        UpdatePositionSkillDto dto,
        CancellationToken cancellationToken);

    Task DeleteAsync(
        int positionId,
        int skillId,
        CancellationToken cancellationToken);

    Task<List<PositionSkillHistoryDto>> GetHistoryAsync(
        int positionId,
        int skillId,
        CancellationToken cancellationToken);
}
