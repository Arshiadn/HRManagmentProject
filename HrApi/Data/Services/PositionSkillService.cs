using System;
using HrApi.Interfaces;
using HrApi.DTOs.Skill.PositionSkills;
using Microsoft.EntityFrameworkCore;
using HrApi.Exceptions;
using HrApi.Models.Skill;
using HrApi.DTOs.Positions;

namespace HrApi.Data.Services;

public class PositionSkillService : IPositionSkillService
{
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public PositionSkillService(HrDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<PositionSkillDto> CreateAsync(
        int positionId,
        CreatePositionSkillDto dto,
        CancellationToken cancellationToken)
    {
        var positionExists = await _context.Positions
            .AnyAsync(
                x => x.Id == positionId &&
                     x.IsActive,
                cancellationToken);

        if (!positionExists)
        {
            throw new NotFoundException(
                "Position was not found.");
        }

        var skillExists = await _context.Skills
            .AnyAsync(
                x => x.Id == dto.SkillId &&
                     x.IsActive,
                cancellationToken);

        if (!skillExists)
        {
            throw new NotFoundException(
                "Skill was not found.");
        }

        var alreadyExists = await _context.PositionSkills
            .AnyAsync(
                x => x.PositionId == positionId &&
                     x.SkillId == dto.SkillId,
                cancellationToken);

        if (alreadyExists)
        {
            throw new BusinessRuleException(
                "This skill is already required for this position.");
        }

        var positionSkill = new PositionSkill
        {
            PositionId = positionId,
            SkillId = dto.SkillId,
            RequiredLevel = dto.RequiredLevel
        };

        _context.PositionSkills.Add(positionSkill);

        await _context.SaveChangesAsync(cancellationToken);

        var skillName = await _context.Skills
            .Where(x => x.Id == dto.SkillId)
            .Select(x => x.Title)
            .FirstAsync(cancellationToken);

        return new PositionSkillDto
        {
            Id = positionSkill.Id,
            PositionId = positionSkill.PositionId,
            SkillId = positionSkill.SkillId,
            SkillName = skillName,
            RequiredLevel = positionSkill.RequiredLevel
        };
    }
    public async Task<List<PositionSkillDto>> GetAllAsync(
        int positionId,
        CancellationToken cancellationToken)
    {
        var positionExists = await _context.Positions
            .AnyAsync(
                x => x.Id == positionId &&
                     x.IsActive,
                cancellationToken);

        if (!positionExists)
        {
            throw new NotFoundException(
                "Position was not found.");
        }

        return await _context.PositionSkills
            .Where(x => x.PositionId == positionId)
            .Select(x => new PositionSkillDto
            {
                Id = x.Id,
                PositionId = x.PositionId,
                SkillId = x.SkillId,
                SkillName = x.Skill.Title,
                RequiredLevel = x.RequiredLevel
            })
            .ToListAsync(cancellationToken);
    }
    public async Task UpdateAsync(
        int positionId,
        int skillId,
        UpdatePositionSkillDto dto,
        CancellationToken cancellationToken)
    {
        var currentUserId = _currentUser.UserId;

        if (string.IsNullOrWhiteSpace(currentUserId))
        {
            throw new UnauthorizedAccessException(
                "User is not authenticated.");
        }

        if (string.IsNullOrWhiteSpace(dto.Reason))
        {
            throw new BusinessRuleException(
                "A reason is required when updating a position skill.");
        }

        var positionSkill = await _context.PositionSkills
            .FirstOrDefaultAsync(
                x => x.PositionId == positionId &&
                    x.SkillId == skillId,
                cancellationToken);

        if (positionSkill is null)
        {
            throw new NotFoundException(
                "Position skill was not found.");
        }

        if (positionSkill.RequiredLevel == dto.RequiredLevel)
        {
            throw new BusinessRuleException(
                "The required skill level is already set to this level.");
        }

        var oldRequiredLevel = positionSkill.RequiredLevel;
        var now = DateTime.UtcNow;

        positionSkill.RequiredLevel = dto.RequiredLevel;

        var history = new PositionSkillHistory
        {
            PositionSkillId = positionSkill.Id,
            OldRequiredLevel = oldRequiredLevel,
            NewRequiredLevel = dto.RequiredLevel,
            Reason = dto.Reason,
            ChangedByUserId = currentUserId,
            ChangedAt = now
        };

        _context.PositionSkillHistories.Add(history);

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task DeleteAsync(
        int positionId,
        int skillId,
        CancellationToken cancellationToken)
    {
        var positionSkill = await _context.PositionSkills
            .FirstOrDefaultAsync(
                x => x.PositionId == positionId &&
                     x.SkillId == skillId,
                cancellationToken);

        if (positionSkill is null)
        {
            throw new NotFoundException(
                "Position skill was not found.");
        }

        _context.PositionSkills.Remove(positionSkill);

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task<List<PositionSkillHistoryDto>> GetHistoryAsync(
    int positionId,
    int skillId,
    CancellationToken cancellationToken)
    {
        var positionSkill = await _context.PositionSkills
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.PositionId == positionId &&
                    x.SkillId == skillId,
                cancellationToken);

        if (positionSkill is null)
        {
            throw new NotFoundException(
                "Position skill was not found.");
        }

        return await _context.PositionSkillHistories
            .AsNoTracking()
            .Where(x => x.PositionSkillId == positionSkill.Id)
            .OrderByDescending(x => x.ChangedAt)
            .Select(x => new PositionSkillHistoryDto
            {
                Id = x.Id,
                PositionSkillId = x.PositionSkillId,
                OldRequiredLevel = x.OldRequiredLevel,
                NewRequiredLevel = x.NewRequiredLevel,
                Reason = x.Reason,
                ChangedByUserId = x.ChangedByUserId,
                ChangedAt = x.ChangedAt
            })
            .ToListAsync(cancellationToken);
    }
}
