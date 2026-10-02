using System;
using HrApi.DTOs.Skill.SkillMatrix;
using HrApi.Models.Skill;

namespace HrApi.Interfaces;

public interface ISkillMatrixService
{
    Task<SkillMatrixDto> GetMineAsync(
        CancellationToken cancellationToken);

    Task<SkillMatrixDto> GetByEmployeeIdAsync(
        int employeeId,
        CancellationToken cancellationToken);

    Task<List<SkillMatrixDto>> GetByDepartmentIdAsync(
        int departmentId,
        CancellationToken cancellationToken);
}
