using System;
using HrApi.Interfaces;
using HrApi.DTOs.Skill.EmployeeSkillClaim;
using HrApi.Exceptions;
using Microsoft.EntityFrameworkCore;
using HrApi.Models.Skill;

namespace HrApi.Data.Services;

public sealed class EmployeeSkillClaimService : IEmployeeSkillClaimService
{
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public EmployeeSkillClaimService(
        HrDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    public async Task<EmployeeSkillClaimDto> CreateAsync(
        CreateEmployeeSkillClaimDto dto,
        CancellationToken cancellationToken)
    {
        var employeeId = _currentUser.EmployeeId;

        if (!employeeId.HasValue)
        {
            throw new BusinessRuleException(
                "The current user is not linked to an employee.");
        }

        var skill = await _context.Skills
            .FirstOrDefaultAsync(
                x => x.Id == dto.SkillId &&
                    x.IsActive,
                    cancellationToken);

        if (skill is null)
        {
            throw new NotFoundException(
                "Skill was not found.");
        }
        if ( dto.EvidenceIds is null || dto.EvidenceIds.Count == 0)
        {
            throw new BusinessRuleException(
                "At least one evidence is required.");
        }
        var evidences = await _context.SkillEvidences
            .Where(x =>
                dto.EvidenceIds.Contains(x.Id) &&
                x.EmployeeId == employeeId.Value &&
                x.SkillId == dto.SkillId)
            .ToListAsync(cancellationToken);

        if (evidences.Count != dto.EvidenceIds.Count)
        {
            throw new BusinessRuleException(
                "One or more evidences are invalid.");
        }
        var claim = new EmployeeSkillClaim
        {
            EmployeeId = employeeId.Value,
            SkillId = dto.SkillId,
            ClaimedLevel = dto.ClaimedLevel,
            IsActive = true
        };
        foreach (var evidence in evidences)
        {
            claim.Evidence.Add(evidence);
        }
        
        _context.EmployeeSkillClaims.Add(claim);

        await _context.SaveChangesAsync(cancellationToken);

        return new EmployeeSkillClaimDto
        {
            Id = claim.Id,
            SkillId = claim.SkillId,
            SkillName = skill.Title,
            ClaimedLevel = claim.ClaimedLevel,
            CreatedAt = claim.CreatedAt,
            EvidenceIds = claim.Evidence
                .Select(x => x.Id)
                .ToList(),
            IsActive = claim.IsActive
        };
    }
    public async Task<List<EmployeeSkillClaimDto>> GetByEmployeeIdAsync(
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

        return await _context.EmployeeSkillClaims
            .Where(x =>
                x.EmployeeId == employeeId &&
                x.IsActive)
            .Select(x => new EmployeeSkillClaimDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                SkillId = x.SkillId,
                SkillName = x.Skill.Title,
                ClaimedLevel = x.ClaimedLevel,
                CreatedAt = x.CreatedAt,
                IsActive = x.IsActive,

                EvidenceIds = x.Evidence
                    .Select(e => e.Id)
                    .ToList()
            })
            .ToListAsync(cancellationToken);
    }
}
