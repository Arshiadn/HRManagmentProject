using System;
using HrApi.DTOs.Skill.SkillEvidences;
using HrApi.Exceptions;
using HrApi.Interfaces;
using Microsoft.EntityFrameworkCore;
using HrApi.Models.Skill;

namespace HrApi.Data.Services;

public class SkillEvidenceService : ISkillEvidenceService
{
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public SkillEvidenceService(HrDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }
    public async Task<SkillEvidenceDto> CreateAsync(
        CreateSkillEvidenceDto dto,
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

        if (dto.ExpiresAt.HasValue &&
            dto.ExpiresAt.Value <= dto.IssuedAt)
        {
            throw new BusinessRuleException(
                "Expiration date must be later than issued date.");
        }

        var evidence = new SkillEvidence
        {
            EmployeeId = employeeId.Value,
            SkillId = dto.SkillId,
            Type = dto.Type,
            IssuedAt = dto.IssuedAt,
            ExpiresAt = dto.ExpiresAt
        };

        _context.SkillEvidences.Add(evidence);

        await _context.SaveChangesAsync(cancellationToken);

        return new SkillEvidenceDto
        {
            Id = evidence.Id,
            SkillId = evidence.SkillId,
            SkillName = skill.Title,
            Type = evidence.Type,
            IssuedAt = evidence.IssuedAt,
            ExpiresAt = evidence.ExpiresAt
        };
    }
    public async Task<List<SkillEvidenceDto>> GetMineAsync(
        CancellationToken cancellationToken)
    {
        var employeeId = _currentUser.EmployeeId;

        if (!employeeId.HasValue)
        {
            throw new BusinessRuleException(
                "The current user is not linked to an employee.");
        }

        return await _context.SkillEvidences
            .Where(x => x.EmployeeId == employeeId.Value)
            .Select(x => new SkillEvidenceDto
            {
                Id = x.Id,
                EmployeeId = employeeId.Value,
                SkillId = x.SkillId,
                SkillName = x.Skill.Title,
                Type = x.Type,
                IssuedAt = x.IssuedAt,
                ExpiresAt = x.ExpiresAt
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<List<SkillEvidenceDto>> GetByEmployeeIdAsync(
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

        return await _context.SkillEvidences
            .Where(x => x.EmployeeId == employeeId)
            .Select(x => new SkillEvidenceDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                SkillId = x.SkillId,
                SkillName = x.Skill.Title,
                Type = x.Type,
                IssuedAt = x.IssuedAt,
                ExpiresAt = x.ExpiresAt
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<SkillEvidenceDto> UpdateAsync(
    long evidenceId,
    UpdateSkillEvidenceDto dto,
    CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            throw new BusinessRuleException(
                "A reason is required when updating skill evidence.");
        }

        if (dto.ExpiresAt.HasValue &&
            dto.ExpiresAt.Value <= dto.IssuedAt)
        {
            throw new BusinessRuleException(
                "Expiration date must be after issued date.");
        }

        var evidence = await _context.SkillEvidences
            .Include(x => x.Skill)
            .FirstOrDefaultAsync(
                x => x.Id == evidenceId,
                cancellationToken);

        if (evidence is null)
        {
            throw new NotFoundException(
                "Skill evidence was not found.");
        }

        var oldIssuedAt = evidence.IssuedAt;
        var oldExpiresAt = evidence.ExpiresAt;
        var oldType = evidence.Type;

        evidence.IssuedAt = dto.IssuedAt;
        evidence.ExpiresAt = dto.ExpiresAt;
        evidence.Type = dto.Type;

        var changedByUserId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(changedByUserId))
        {
            throw new BusinessRuleException(
                "The current user is not authenticated.");
        }

        var history = new SkillEvidenceHistory
        {
            SkillEvidenceId = evidence.Id,

            OldIssuedAt = oldIssuedAt,
            NewIssuedAt = evidence.IssuedAt,

            OldExpiresAt = oldExpiresAt,
            NewExpiresAt = evidence.ExpiresAt,

            OldType = oldType,
            NewType = evidence.Type,

            Reason = dto.Reason,
            ChangedByUserId = changedByUserId,
            ChangedAt = DateTime.UtcNow
        };

        _context.EvidenceHistories.Add(history);

        await _context.SaveChangesAsync(cancellationToken);

        return new SkillEvidenceDto
        {
            Id = evidence.Id,
            EmployeeId = evidence.EmployeeId,
            SkillId = evidence.SkillId,
            SkillName = evidence.Skill.Title,
            Type = evidence.Type,
            IssuedAt = evidence.IssuedAt,
            ExpiresAt = evidence.ExpiresAt
        };
    }
}
