using System;
using HrApi.DTOs.Skill.History;

namespace HrApi.Interfaces;

public interface ISkillStateHistoryService
{
    Task<List<SkillStateHistoryDto>> GetByEmployeeAndSkillAsync(
    int employeeId,
    int skillId,
    CancellationToken cancellationToken);

    Task<List<EmployeeSkillHistoryDto>> GetByEmployeeIdAsync(
    int employeeId,
    CancellationToken cancellationToken);
}
