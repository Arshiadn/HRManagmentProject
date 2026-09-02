using HrApi.DTOs.ShiftAssignment;
using HrApi.DTOs.ValueObjects;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class EmployeeShiftAssignmentService
    : IEmployeeShiftAssignmentService
{
    private readonly HrDbContext _context;
    public EmployeeShiftAssignmentService(HrDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftAssignmentDetailsDto> AssignShiftAsync(
        int employeeId,
        CreateShiftAssignmentDto request,
        CancellationToken cancellationToken)
    {
        if(request.EffectiveTo.HasValue &&
            request.EffectiveTo.Value < request.EffectiveFrom)
        {
            throw new ArgumentException(
                "EffectiveTo date cannot be earlier than EffectiveFrom date.");
        }

        var employeeExists = await _context.Employees
            .AnyAsync(e => e.Id == employeeId, cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException(
                $"There is no employee with ID {employeeId}.");
        }

        var shift = await _context.Shifts
           .FirstOrDefaultAsync(
               x => x.Id == request.ShiftId,
               cancellationToken);

        if (shift is null)
        {
            throw new NotFoundException(
                $"There is no shift with ID {request.ShiftId}.");
        }

        if (!shift.IsActive)
        {
            throw new BusinessRuleException(
                "The selected shift is not active.");
        }
        var hasOverLap = await _context.ShiftAssignments
            .AnyAsync(x =>
            x.EmployeeId == employeeId &&
            x.EffectiveFrom <= (request.EffectiveTo ?? DateOnly.MaxValue) &&
            (x.EffectiveTo ?? DateOnly.MaxValue) >= request.EffectiveFrom,
            cancellationToken);

        if (hasOverLap)
        {
            throw new ConflictException(
                "the new shift assignment overlaps with an existing assignmet");
        }

        var assigmnet = new EmployeeShiftAssignment
        {
            EmployeeId = employeeId,
            ShiftId = request.ShiftId,
            EffectiveFrom = request.EffectiveFrom,
            EffectiveTo = request.EffectiveTo
        };
        await _context.ShiftAssignments.AddAsync(assigmnet, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(assigmnet, shift);
    }

    public async Task<IReadOnlyList<ShiftAssignmentDetailsDto>> GetAssignmentsAsync(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
            .AnyAsync(x => x.Id == employeeId, cancellationToken);

        if (!employeeExists)
        {
            throw new NotFoundException(
                $"There is no employee with ID {employeeId}.");
        }

        var assignment = await _context.ShiftAssignments
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Shift)
            .OrderByDescending(x => x.EffectiveFrom)
            .Select(x => new ShiftAssignmentDetailsDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                ShiftId = x.ShiftId,
                ShiftName = x.Shift.Name,
                WorkingHours = new TimeRangeDto
                {
                    Start = x.Shift.WorkingHours.Start,
                    End = x.Shift.WorkingHours.End
                },
                GraceMinutes = x.Shift.GraceMinutes,
                EffectiveFrom = x.EffectiveFrom,
                EffectiveTo = x.EffectiveTo
            })
            .ToListAsync(cancellationToken);

        return assignment;
    }
    private static ShiftAssignmentDetailsDto MapToDto(
        EmployeeShiftAssignment assignment,
        Shift shift)
    {
        return new ShiftAssignmentDetailsDto
        {
            Id = assignment.Id,
            EmployeeId = assignment.EmployeeId,
            ShiftId = assignment.ShiftId,
            ShiftName = assignment.Shift.Name,
            WorkingHours = new TimeRangeDto
            {
                Start = shift.WorkingHours.Start,
                End = shift.WorkingHours.End
            },
            GraceMinutes = shift.GraceMinutes,

            EffectiveFrom = assignment.EffectiveFrom,
            EffectiveTo = assignment.EffectiveTo
        };
    }
}
