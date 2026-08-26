using HrApi.DTOs.Candidates;
using HrApi.DTOs.Interviews;
using HrApi.Enums;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Cryptography;

namespace HrApi.Data.Services;

public class RecruitmentService : IRecruitmentService
{
    private readonly HrDbContext _context;
    private readonly ICurrentUserService _currentUserService;
    public RecruitmentService
        (HrDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }
    private static bool CanTransition(
        RecruitmentStage from,
        RecruitmentStage to)
    {
        return (from, to) switch
        {
            (RecruitmentStage.New,
            RecruitmentStage.InterviewScheduled) => true,

            (RecruitmentStage.New,
            RecruitmentStage.Rejected) => true,

            (RecruitmentStage.InterviewScheduled,
            RecruitmentStage.Interviewed) => true,

            (RecruitmentStage.InterviewScheduled,
            RecruitmentStage.Rejected) => true,

            (RecruitmentStage.Interviewed,
            RecruitmentStage.Accepted) => true,

            (RecruitmentStage.Interviewed,
            RecruitmentStage.Rejected) => true,

            (RecruitmentStage.Accepted,
            RecruitmentStage.Hired) => true,

            (RecruitmentStage.Accepted,
            RecruitmentStage.Rejected) => true,

            _ => false
        };
    }
    private void ChangeStage(
        Candidate candidate,
        RecruitmentStage targetStage,
        string? reason)
    {
        var currentStage = candidate.Stage;

        if (!CanTransition(currentStage, targetStage))
        {
            throw new BusinessRuleException(
                $"Transition from {currentStage} " +
                $"to {targetStage} is not allowed.");
        }

        candidate.Stage = targetStage;
        candidate.UpdatedAtUtc = DateTime.UtcNow;

        candidate.StageHistory.Add(new RecruitmentStageHistory
        {
            FromStage = currentStage,
            ToStage = targetStage,
            Reason = reason,
            ChangedAtUtc = DateTime.UtcNow,
            ChangedByUserId = _currentUserService.UserId
        });
    }
    public async Task ScheduleInterviewAsync(
    int candidateId,
    ScheduleInterviewRequest request,
    CancellationToken cancellationToken)
    {
        var candidate = await _context.Candidates
            .Include(c => c.StageHistory)
            .FirstOrDefaultAsync(c => c.Id == candidateId);

        if (candidate is null)
        {
            throw new NotFoundException("Candidate not found.");
        }

        if (request.ScheduledAtUtc <= DateTime.UtcNow)
        {
            throw new BusinessRuleException(
                "Interview time must be in the future.");
        }
        ChangeStage(
            candidate,
            RecruitmentStage.InterviewScheduled,
            "Interview scheduled");

        candidate.Interviews.Add(new Interview
        {
            ScheduledAtUtc = request.ScheduledAtUtc,
            InterviewerName = request.InterviewerName.Trim(),
            InterviewType = request.InterviewType.Trim(),
            Notes = request.Notes?.Trim(),
            CreatedAtUtc = DateTime.UtcNow
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task CompleteInterviewAsync(
        int candidateId,
        CompleteInterviewRequest request,
        CancellationToken cancellationToken)
    {
        var candidate = await _context.Candidates
            .Include(x => x.Interviews)
            .Include(x => x.StageHistory)
            .FirstOrDefaultAsync(
                x => x.Id == candidateId,
                cancellationToken);
        if (candidate is null)
        {
            throw new NotFoundException("Candidate not found.");
        }
        if (candidate.Stage != RecruitmentStage.InterviewScheduled)
        {
            throw new BusinessRuleException(
                "Candidate is not waiting for an interview.");
        }
        var interview = candidate.Interviews
            .OrderByDescending(x => x.ScheduledAtUtc)
            .FirstOrDefault();

        if (interview is null)
        {
            throw new BusinessRuleException(
                "No scheduled interview was found.");
        }

        interview.Score = request.Score;
        interview.Notes = request.Notes?.Trim();

        ChangeStage(
            candidate,
            RecruitmentStage.Interviewed,
            "Interview completed");

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task AcceptCandidateAsync(
        int candidateId,
        CancellationToken cancellationToken)
    {
        var candidate = await _context.Candidates
                .Include(c => c.StageHistory)
                .FirstOrDefaultAsync(c => c.Id == candidateId,
                cancellationToken);

        if (candidate is null)
        {
            throw new NotFoundException("Candidate not found.");
        }
        ChangeStage(
            candidate,
            RecruitmentStage.Accepted,
            "Candidate accepted");

        await _context.SaveChangesAsync(cancellationToken);
    }
    public async Task RejectCandidateAsync(
        int candidateId,
        RejectCandidateRequest request,
        CancellationToken cancellationToken)
    {
        var candidate = await _context.Candidates
                .Include(c => c.StageHistory)
                .FirstOrDefaultAsync(c => c.Id == candidateId,
                cancellationToken);

        if (candidate is null)
        {
            throw new NotFoundException("Candidate not found.");
        }

        candidate.RejectionReason = request.Reason.Trim();

        ChangeStage(
            candidate,
            RecruitmentStage.Rejected,
            request.Reason.Trim());

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> HireCandidateAsync(
        int candidateId,
        HireCandidateRequest request,
        CancellationToken cancellationToken)
    {
        await using var transaction = await _context.Database
            .BeginTransactionAsync(cancellationToken);

        try
        {
            var candidate = await _context.Candidates
                .Include(c => c.StageHistory)
                .FirstOrDefaultAsync(c => c.Id == candidateId,
                cancellationToken);

            if (candidate is null)
            {
                throw new NotFoundException("Candidate not found.");
            }

            if (candidate.Stage != RecruitmentStage.Accepted)
            {
                throw new BusinessRuleException(
                    "Only accepted candidates can be hired.");
            }

            if (candidate.EmployeeId.HasValue)
            {
                throw new BusinessRuleException(
                    "Candidate has already been hired.");
            }
            
            var departmentExists = await _context.Departments
                .AnyAsync(d => d.Id == request.DepartmentId && d.IsActive,
                cancellationToken);

            if (!departmentExists)
            {
                throw new BusinessRuleException(
                    "Department is invalid or inactive.");
            }

            var personnelCodeExists = await _context.Employees
                .AnyAsync(e => e.PersonnelCode == request.PersonnelCode,
                cancellationToken);

            if (personnelCodeExists)
            {
                throw new BusinessRuleException(
                    "Employee code already exists.");
            }

            var employee = new Employee
            {
                FullName = candidate.FullName.Trim(),
                Email = candidate.Email,
                DepartmentId = request.DepartmentId,
                PersonnelCode = request.PersonnelCode,
                HireDateFrom = request.HireDate,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Employees.Add(employee);

            await _context.SaveChangesAsync(cancellationToken);

            candidate.EmployeeId = employee.Id;
            candidate.HiredAtUtc = DateTime.UtcNow;

            ChangeStage(
                candidate,
                RecruitmentStage.Hired,
                "Candidate hired as employee");

            await _context.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return employee.Id;
        }
        catch
        {
            await transaction
                .RollbackAsync(cancellationToken);

            throw;
        }
    }
    public async Task ReopenCandidateAsync(
        int candidateId,
        CancellationToken cancellationToken)
    {
        var candidate = await _context.Candidates
                .Include(c => c.StageHistory)
                .FirstOrDefaultAsync(c => c.Id == candidateId,
                cancellationToken);

        if (candidate is null)
        {
            throw new NotFoundException("Candidate not found.");
        }

        if(candidate.Stage != RecruitmentStage.Rejected)
        {
            throw new BusinessRuleException(
                "Only rejected candidates can be reopened.");
        }
        var fromStage = candidate.Stage;
        candidate.Stage = RecruitmentStage.New;
        candidate.RejectionReason = null;
        candidate.UpdatedAtUtc = DateTime.UtcNow;

        candidate.StageHistory.Add(new RecruitmentStageHistory
        {
            FromStage = fromStage,
            ToStage = RecruitmentStage.New,
            Reason = "Candidate reopen",
            ChangedAtUtc = DateTime.UtcNow,
            ChangedByUserId = _currentUserService.UserId
        });

        await _context.SaveChangesAsync(cancellationToken);
    }
}
