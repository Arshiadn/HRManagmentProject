using System;
using HrApi.DTOs.Skill.SkillAssessments;

namespace HrApi.Interfaces;

public interface ISkillAssessmentService
{
    Task<SkillAssessmentDto> CreateAsync(
        CreateSkillAssessmentDto dto,
        CancellationToken cancellationToken);
}
