using System;
using HrApi.DTOs.Skill.History;
using HrApi.Exceptions;
using HrApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class SkillStateHistoryService : ISkillStateHistoryService
{
    private readonly HrDbContext _context;

    public SkillStateHistoryService(HrDbContext context)
    {
        _context = context;
    }

    public async Task<List<SkillStateHistoryDto>> GetByEmployeeAndSkillAsync(
        int employeeId,
        int skillId,
        CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(
                x => x.Id == employeeId &&
                     x.IsActive,
                cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException(
                "Employee was not found.");
        }

        var skillExists = await _context.Skills
            .AnyAsync(
                x => x.Id == skillId &&
                     x.IsActive,
                cancellationToken);

        if (!skillExists)
        {
            throw new NotFoundException(
                "Skill was not found.");
        }

        return await _context.SkillStateHistories
            .AsNoTracking()
            .Where(x =>
                x.EmployeeId == employeeId &&
                x.SkillId == skillId)
            .OrderByDescending(x => x.OccurredAt)
            .Select(x => new SkillStateHistoryDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                SkillId = x.SkillId,
                SkillName = x.Skill.Title,
                FromLevel = x.FromLevel,
                ToLevel = x.ToLevel,
                SkillAssessmentId = x.SkillAssessmentId,
                Reason = x.Reason,
                OccurredAt = x.OccurredAt
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<List<EmployeeSkillHistoryDto>> GetByEmployeeIdAsync(
    int employeeId,
    CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(
                x => x.Id == employeeId &&
                    x.IsActive,
                cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException(
                "Employee was not found.");
        }

        var histories = await _context.SkillStateHistories
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .OrderBy(x => x.SkillId)
            .ThenByDescending(x => x.OccurredAt)
            .Select(x => new
            {
                x.SkillId,
                SkillName = x.Skill.Title,
                History = new SkillStateHistoryDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    SkillId = x.SkillId,
                    SkillName = x.Skill.Title,
                    FromLevel = x.FromLevel,
                    ToLevel = x.ToLevel,
                    SkillAssessmentId = x.SkillAssessmentId,
                    Reason = x.Reason,
                    OccurredAt = x.OccurredAt
                }
            })
            .ToListAsync(cancellationToken);

        return histories
            .GroupBy(x => new
            {
                x.SkillId,
                x.SkillName
            })
            .Select(x => new EmployeeSkillHistoryDto
            {
                SkillId = x.Key.SkillId,
                SkillName = x.Key.SkillName,
                History = x
                    .Select(y => y.History)
                    .ToList()
            })
            .ToList();
    }
}
