using System;
using HrApi.DTOs.Skill.SkillAssessments;
using HrApi.Enums.Skill;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models.Skill;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class SkillAssessmentService : ISkillAssessmentService
{
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public SkillAssessmentService(
        HrDbContext context,
        ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<SkillAssessmentDto> CreateAsync(
        CreateSkillAssessmentDto dto, 
        CancellationToken cancellationToken)
    {
        var assessorId = _currentUser.UserId;

        if(string.IsNullOrEmpty(assessorId))
            throw new UnauthorizedAccessException("User is not authenticated.");

        var claim = await _context.EmployeeSkillClaims
            .Include(x => x.Evidence)
            .FirstOrDefaultAsync(
                x => x.Id == dto.SkillClaimId &&
                x.IsActive, cancellationToken);

        if(claim is null)
            throw new NotFoundException("Skill claim was not found.");

        if (claim.Evidence.Count == 0)
            throw new BusinessRuleException("A skill claim must have at least one evidence");

        var assessmentExists = await _context.SkillAssessments
            .AnyAsync(
                x => x.SkillClaimId == claim.Id,
                cancellationToken);

        if (assessmentExists)
        {
            throw new BusinessRuleException(
                "This skill claim has already been assessed.");
        }

        var now = DateTime.UtcNow;

        var validEvidenceExists = claim.Evidence.Any(
            x => !x.ExpiresAt.HasValue || x.ExpiresAt.Value > now);
        
        if(!validEvidenceExists)
            throw new BusinessRuleException(
                "The skill claim has no valid evidence.");

        if (dto.Decision == SkillLevel.Unassessed)
        {
            throw new BusinessRuleException(
                "Assessment decision cannot be Unassessed.");
        }

        var assessment = new SkillAssessment
        {
          SkillClaimId = claim.Id,
          Decision = dto.Decision,
          AssessorId = assessorId,
          AssessedAt = now,
          Comment = dto.Comment  
        };

        _context.SkillAssessments.Add(assessment);

        claim.IsActive = false;

        var currentState = await _context.EmployeeSkillStates
            .FirstOrDefaultAsync(x => x.EmployeeId == claim.EmployeeId &&
            x.SkillId == claim.SkillId, cancellationToken);

        var previousLevel = currentState?.CurrentLevel;

        if(currentState is null)
        {
            currentState = new EmployeeSkillState
            {
                EmployeeId = claim.EmployeeId,
                SkillId = claim.SkillId,
                CurrentLevel = dto.Decision,
                UpdatedAt = now
            };

            _context.EmployeeSkillStates.Add(currentState);
        }
        else
        {
            currentState.CurrentLevel = dto.Decision;
            currentState.UpdatedAt = now;
        }

        var history = new SkillStateHistory
        {
            EmployeeId = claim.EmployeeId,
            SkillId = claim.SkillId,
            FromLevel = previousLevel,
            ToLevel = dto.Decision,
            SkillAssessment = assessment,
            Reason = "Skill assessment completed.",
            OccurredAt = now
        };

        _context.SkillStateHistories.Add(history);

        await _context.SaveChangesAsync(cancellationToken);

        return new SkillAssessmentDto
        {
            Id = assessment.Id,
            SkillClaimId = assessment.SkillClaimId,
            EmployeeId = claim.EmployeeId,
            SkillId = claim.SkillId,
            Decision = assessment.Decision,
            AssessorId = assessment.AssessorId,
            AssessedAt = assessment.AssessedAt,
            Comment = assessment.Comment
        };
    }
}
