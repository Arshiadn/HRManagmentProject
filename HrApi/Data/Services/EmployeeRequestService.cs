using HrApi.DTOs.EmployeeRequests;
using HrApi.DTOs.Paging;
using HrApi.Enums.Request;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Policies.Request;
using HrApi.Responses;
using HrApi.Specifications;
using HrApi.Specifications.Employee.Requests;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class EmployeeRequestService : IEmployeeRequestService
{
    private readonly HrDbContext _context;
    private readonly RequestPolicyResolver _policyResolver;

    public EmployeeRequestService(
        RequestPolicyResolver policyResolver,
        HrDbContext context)
    {
        _policyResolver = policyResolver;
        _context = context;
    }

    public async Task<EmployeeRequestDetailsDto> CreateAsync(
        CreateEmployeeRequestDto dto,
        CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(x => x.Id == dto.EmployeeId);

        if (!employeeExists)
        {
            throw new NotFoundException(
            $"Employee with id {dto.EmployeeId} was not found.");
        }

        if (dto.ToDate < dto.FromDate)
        {
            throw new BusinessRuleException(
                "ToDate cannot be earlier than FromDate.");
        }

        var request = new EmployeeRequest
        {
            EmployeeId = dto.EmployeeId,
            Type = dto.Type,
            FromDate = dto.FromDate,
            ToDate = dto.ToDate,
            Destination = dto.Destination,
            Purpose = dto.Purpose,
            AttachmentPath = dto.AttachmentPath
        };

        _context.EmployeeRequests.Add(request);

        await _context.SaveChangesAsync(
            cancellationToken);

        await _context.Entry(request)
            .ReloadAsync(cancellationToken);

        return MapToDto(request);
    }
    public async Task SubmitAsync(
        long Id,
        CancellationToken cancellationToken)
    {
        var request = await _context.EmployeeRequests
            .FirstOrDefaultAsync(
                x => x.Id == Id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Request with Id {Id} was not found.");

        if (request.Status != RequestStatus.Draft)
            throw new ConflictException(
                "Only draft requests can be submitted.");

        var policy =
            _policyResolver.Resolve(request.Type);

        await policy.ValidateAsync(
            request,
            cancellationToken);

        request.Submit();

        await _context.SaveChangesAsync(
            cancellationToken);
    }
    public async Task ApproveAsync(
        long Id,
        CancellationToken cancellationToken)
    {
        var request = await _context.EmployeeRequests
            .FirstOrDefaultAsync(
                x => x.Id == Id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Request with Id {Id} was not found.");

        if (request.Status != RequestStatus.Submitted)
            throw new ConflictException(
                "Only Submitted requests can be Approve.");

        var policy =
            _policyResolver.Resolve(request.Type);

        await policy.ValidateAsync(
            request,
            cancellationToken);

        request.Approve();

        await _context.SaveChangesAsync(
            cancellationToken);
    }
    public async Task RejectAsync(
        long Id,
        CancellationToken cancellationToken)
    {
        var request = await _context.EmployeeRequests
            .FirstOrDefaultAsync(
                x => x.Id == Id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Request with Id {Id} was not found.");

        if (request.Status != RequestStatus.Submitted)
            throw new ConflictException(
                "Only Submitted requests can be Reject.");

        var policy =
            _policyResolver.Resolve(request.Type);

        await policy.ValidateAsync(
            request,
            cancellationToken);

        request.Reject();

        await _context.SaveChangesAsync(
            cancellationToken);
    }
    public async Task CancelAsync(
        long Id,
        CancellationToken cancellationToken)
    {
        var request = await _context.EmployeeRequests
            .FirstOrDefaultAsync(
                x => x.Id == Id,
                cancellationToken)
            ?? throw new NotFoundException(
                $"Request with Id {Id} was not found.");

        if (request.Status != RequestStatus.Submitted)
            throw new ConflictException(
                "Only Submitted requests can be Cancel.");

        var policy =
            _policyResolver.Resolve(request.Type);

        await policy.ValidateAsync(
            request,
            cancellationToken);

        request.Cancel();

        await _context.SaveChangesAsync(
            cancellationToken);
    }
    public async Task<ApiResponse<EmployeeRequestDetailsDto>> GetById(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
        .AnyAsync(
            x => x.Id == employeeId,
            cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException(
                $"Employee with id {employeeId} was not found.");
        }

        var request = await _context.EmployeeRequests
        .AsNoTracking()
        .Where(x => x.EmployeeId == employeeId)
        .OrderByDescending(x => x.Id)
        .Select(x => new EmployeeRequestDetailsDto
        {
            Id = x.Id,
            EmployeeId = x.EmployeeId,
            Type = x.Type,
            Status = x.Status,
            FromDate = x.FromDate,
            ToDate = x.ToDate,
            TotalDays = x.TotalDays,
            Destination = x.Destination,
            Purpose = x.Purpose,
            AttachmentPath = x.AttachmentPath
        })
        .FirstOrDefaultAsync(cancellationToken)
        ?? throw new NotFoundException(
                $"Request was not found."); ;

        return new ApiResponse<EmployeeRequestDetailsDto>
        {
            Success = true,
            Message = $"Employee's Request with Id {request.Id} has been retrieved",
            Data = request
        };
    }
    public async Task<PagedResultDto<EmployeeRequestDetailsDto>> GetListAsync(
        EmployeeRequestListRequest request,
        CancellationToken cancellationToken)
    {
        var listSpecification =
            new EmployeeRequestListSpecification(request);

        var countSpecification = 
            new EmployeeRequestCountSpecification(request);

        var query = SpecificationEvaluator.GetQuery(
            _context.EmployeeRequests,
            listSpecification);

        var countQuery = SpecificationEvaluator.GetQuery(
            _context.EmployeeRequests,
            countSpecification);

        var totalCount = await countQuery.CountAsync(cancellationToken);

        var items = await query
        .Select(x => new EmployeeRequestDetailsDto
        {
            Id = x.Id,
            EmployeeId = x.EmployeeId,
            Type = x.Type,
            Status = x.Status,
            FromDate = x.FromDate,
            ToDate = x.ToDate,
            TotalDays = x.TotalDays,
            Destination = x.Destination,
            Purpose = x.Purpose,
            AttachmentPath = x.AttachmentPath
        })
        .ToListAsync(cancellationToken);

        return new PagedResultDto<EmployeeRequestDetailsDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount,
            TotalPages = (int)Math.Ceiling(
                totalCount / (double)request.PageSize)
        };
    }
    private static EmployeeRequestDetailsDto MapToDto(
        EmployeeRequest request)
    {
        return new EmployeeRequestDetailsDto
        {
            Id = request.Id,
            EmployeeId = request.EmployeeId,
            Type = request.Type,
            Status = request.Status,
            FromDate = request.FromDate,
            ToDate = request.ToDate,
            TotalDays = request.TotalDays,
            Destination = request.Destination,
            Purpose = request.Purpose,
            AttachmentPath = request.AttachmentPath
        };
    }
}
