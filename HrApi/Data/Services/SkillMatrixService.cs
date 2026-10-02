using System;
using HrApi.DTOs.Skill.SkillMatrix;
using HrApi.Enums.Skill;
using HrApi.Exceptions;
using HrApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public class SkillMatrixService : ISkillMatrixService
{
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _userService;

    public SkillMatrixService(HrDbContext context, ICurrentUserService userService)
    {
        _context = context;
        _userService = userService;
    }

     public async Task<SkillMatrixDto> GetMineAsync(
        CancellationToken cancellationToken)
    {
        var employeeId = _userService.EmployeeId;

        if (!employeeId.HasValue)
        {
            throw new BusinessRuleException(
                "The current user is not associated with an employee.");
        }

        return await BuildMatrixAsync(
            employeeId.Value,
            cancellationToken);
    }

    public async Task<SkillMatrixDto> GetByEmployeeIdAsync(
        int employeeId,
        CancellationToken cancellationToken)
    {
        return await BuildMatrixAsync(
            employeeId,
            cancellationToken);
    }
    // Needs query optimization
    public async Task<List<SkillMatrixDto>> GetByDepartmentIdAsync(
        int departmentId,
        CancellationToken cancellationToken)
    {
        var departmentExists = await _context.Departments
            .AsNoTracking()
            .AnyAsync(
                x => x.Id == departmentId,
                cancellationToken);

        if (!departmentExists)
        {
            throw new NotFoundException(
                "Department was not found.");
        }

        var employees = await _context.Employees
            .AsNoTracking()
            .Where(x =>
                x.DepartmentId == departmentId &&
                x.IsActive)
            .Select(x => new
            {
                x.Id,
                x.FullName,
                x.PositionId,
                PositionTitle = x.Position.Title
            })
            .ToListAsync(cancellationToken);

        if (employees.Count == 0)
        {
            return new List<SkillMatrixDto>();
        }

        var positionIds = employees
            .Select(x => x.PositionId)
            .Distinct()
            .ToList();

        var positionSkills = await _context.PositionSkills
            .AsNoTracking()
            .Where(x => positionIds.Contains(x.PositionId))
            .Select(x => new
            {
                x.PositionId,
                x.SkillId,
                SkillName = x.Skill.Title,
                x.RequiredLevel
            })
            .ToListAsync(cancellationToken);

        var employeeIds = employees
            .Select(x => x.Id)
            .ToList();

        var currentStates = await _context.EmployeeSkillStates
            .AsNoTracking()
            .Where(x =>
                employeeIds.Contains(x.EmployeeId))
            .Select(x => new
            {
                x.EmployeeId,
                x.SkillId,
                x.CurrentLevel
            })
            .ToListAsync(cancellationToken);

        var currentStateLookup = currentStates
            .ToDictionary(
                x => (x.EmployeeId, x.SkillId),
                x => x.CurrentLevel);

        return employees
            .Select(employee =>
            {
                var skills = positionSkills
                    .Where(x => x.PositionId == employee.PositionId)
                    .Select(skill =>
                    {
                        var currentLevel =
                            currentStateLookup.TryGetValue(
                                (employee.Id, skill.SkillId),
                                out var level)
                                ? level
                                : SkillLevel.Unassessed;

                        var status = currentLevel >= skill.RequiredLevel
                            ? SkillMatrixStatus.Meets
                            : SkillMatrixStatus.Gap;

                        return new SkillMatrixItemDto
                        {
                            SkillId = skill.SkillId,
                            SkillName = skill.SkillName,
                            RequiredLevel = skill.RequiredLevel,
                            CurrentLevel = currentLevel,
                            Status = status
                        };
                    })
                    .ToList();

                return new SkillMatrixDto
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.FullName,
                    PositionId = employee.PositionId,
                    PositionTitle = employee.PositionTitle,
                    Skills = skills
                };
            })
            .ToList();
    }
    private async Task<SkillMatrixDto> BuildMatrixAsync(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Include(x => x.Position)
            .FirstOrDefaultAsync(
                x => x.Id == employeeId &&
                     x.IsActive,
                cancellationToken);

        if (employee is null)
        {
            throw new NotFoundException(
                "Employee was not found.");
        }

        var positionSkills = await _context.PositionSkills
            .Where(x => x.PositionId == employee.PositionId)
            .Select(x => new
            {
                x.SkillId,
                SkillName = x.Skill.Title,
                x.RequiredLevel
            })
            .ToListAsync(cancellationToken);

        var skillIds = positionSkills
            .Select(x => x.SkillId)
            .ToList();

        var currentStates = await _context.EmployeeSkillStates
            .Where(x =>
                x.EmployeeId == employee.Id &&
                skillIds.Contains(x.SkillId))
            .Select(x => new
            {
                x.SkillId,
                x.CurrentLevel
            })
            .ToDictionaryAsync(
                x => x.SkillId,
                x => x.CurrentLevel,
                cancellationToken);

        var skills = positionSkills
            .Select(x =>
            {
                var currentLevel = currentStates.TryGetValue(
                    x.SkillId,
                    out var level)
                    ? level
                    : SkillLevel.Unassessed;

                var status = currentLevel >= x.RequiredLevel
                    ? SkillMatrixStatus.Meets
                    : SkillMatrixStatus.Gap;

                return new SkillMatrixItemDto
                {
                    SkillId = x.SkillId,
                    SkillName = x.SkillName,
                    RequiredLevel = x.RequiredLevel,
                    CurrentLevel = currentLevel,
                    Status = status
                };
            })
            .ToList();

        return new SkillMatrixDto
        {
            EmployeeId = employee.Id,
            EmployeeName = employee.FullName,
            PositionId = employee.PositionId,
            PositionTitle = employee.Position.Title,
            Skills = skills
        };
    }
}
