using HrApi.DTOs.Candidates;
using HrApi.DTOs.Interviews;
using HrApi.DTOs.Paging;
using HrApi.DTOs.StageHistory;
using HrApi.Enums;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Responses;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public class CandidateService : ICandidateServicecs
{
    private readonly HrDbContext _context;
    public CandidateService(HrDbContext context) => _context = context;

    public async Task<ApiResponse<CandidateDetailsDto>>
        GetByIdAsync(int id,
        CancellationToken cancellationToken)
    {
        var candidate = await _context.Candidates
            .Where(c => c.Id == id)
            .AsNoTracking()
            .Select(c => new CandidateDetailsDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Stage = c.Stage,
                RejectionReason = c.RejectionReason,
                CreatedAtUtc = c.CreatedAtUtc,
                Interviews = c.Interviews
                    .Select(i => new InterviewDto
                    {
                        Id = i.Id,
                        CandidateId = i.CandidateId,
                        ScheduledAtUtc = i.ScheduledAtUtc,
                        InterviewType = i.InterviewType,
                        InterviewerName = i.InterviewerName,
                        CreatedAtUtc = i.CreatedAtUtc,
                        Notes = i.Notes,
                        Score = i.Score
                    })
                    .ToList(),
                StageHistory = c.StageHistory
                    .Select(s => new StageHistoryDto
                    {
                        Id = s.Id,
                        CandidateId = s.CandidateId,
                        FromStage = s.FromStage,
                        ToStage = s.ToStage,
                        Reason = s.Reason,
                        ChangedAtUtc = s.ChangedAtUtc,
                        ChangedByUserId = s.ChangedByUserId
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);

        if(candidate is null)
        {
            throw new NotFoundException("Candidate did not found");
        }

        return new ApiResponse<CandidateDetailsDto>
        {
            Success = true,
            Message = "Candidate info recieved",
            Data = candidate
        };
    }

    public async Task<PagedResultDto<CandidateDetailsDto>> GetListAsync(
        CandidateListRequest request,
        CancellationToken cancellationToken)
    {
        var query = _context.Candidates
            .AsNoTracking()
            .AsQueryable();
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            var search = request.Search.Trim();
            query = query.Where(c =>
                c.FullName.Contains(search) ||
                c.Email.Contains(search) || 
                c.PhoneNumber.Contains(search));
        }
        if (request.Stage.HasValue)
            query = query.Where(c =>
                c.Stage == request.Stage.Value);

        if (request.CreatedAtUtc.HasValue)
            query = query.Where(c =>
                c.CreatedAtUtc <= request.CreatedAtUtc.Value);

        var totalItems = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(c => new CandidateDetailsDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Stage = c.Stage,
                RejectionReason = c.RejectionReason,
                CreatedAtUtc = c.CreatedAtUtc,
                Interviews = c.Interviews
                    .Select(i => new InterviewDto
                    {
                        Id = i.Id,
                        CandidateId = i.CandidateId,
                        ScheduledAtUtc = i.ScheduledAtUtc,
                        InterviewType = i.InterviewType,
                        InterviewerName = i.InterviewerName,
                        CreatedAtUtc = i.CreatedAtUtc,
                        Notes = i.Notes,
                        Score = i.Score
                    })
                    .ToList(),
                StageHistory = c.StageHistory
                    .Select(s => new StageHistoryDto
                    {
                        Id = s.Id,
                        CandidateId = s.CandidateId,
                        FromStage = s.FromStage,
                        ToStage = s.ToStage,
                        Reason = s.Reason,
                        ChangedAtUtc = s.ChangedAtUtc,
                        ChangedByUserId = s.ChangedByUserId
                    })
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return new PagedResultDto<CandidateDetailsDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalItems,
            TotalPages = (int)Math.Ceiling(
                totalItems / (double)request.PageSize)
        };
    }
    public async Task<int> CreateCandidateAsync(
    CreateCandidateRequest request,
    CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();

        var emailExists = await _context.Candidates
            .AnyAsync(c => c.Email == normalizedEmail &&
             c.Stage != RecruitmentStage.Rejected,
             cancellationToken);
        if (emailExists)
        {
            throw new BusinessRuleException(
                "An active candidate with this email already exists.");
        }

        var candidate = new Candidate
        {
            FullName = request.FullName.Trim(),
            Email = normalizedEmail,
            PhoneNumber = request.PhoneNumber,
            CreatedAtUtc = DateTime.UtcNow,
            Stage = RecruitmentStage.New
        };

        _context.Candidates.Add(candidate);

        await _context.SaveChangesAsync(cancellationToken);

        return candidate.Id;
    }
}
