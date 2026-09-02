using HrApi.Calculators;
using HrApi.DTOs.Attendance;
using HrApi.DTOs.Paging;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Setting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Threading;

namespace HrApi.Data.Services;

public sealed class AttendanceService : IAttendanceService
{
    private readonly HrDbContext _context;
    private readonly TimeZoneInfo _companyTimeZone;
    public AttendanceService(
        HrDbContext context)
    {
        _context = context;
        _companyTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Asia/Tehran");
    }

    public async Task<AttendanceDailyDto> CheckIn(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;

        var workDate = WorkTimeCalculator.GetWorkDate(
            now, _companyTimeZone);

        var attendance = await _context.AttendanceRecords
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId &&
                x.WorkDate == workDate,
                cancellationToken);

        if(attendance is not null && attendance.CheckInAt.HasValue)
        {
            throw new ConflictException(
                "Employee has already checked in.");
        }

        var assignment = await GetCurrentAssignment(
            employeeId,
            workDate, cancellationToken);

        if(assignment is null)
        {
            throw new BusinessRuleException(
                "Employee does not have an active shift");
        }

        var localTime = TimeZoneInfo.ConvertTime(
            now,_companyTimeZone);

        var late = WorkTimeCalculator.CalculateLateArrival(
            TimeOnly.FromDateTime(localTime.DateTime),
            assignment.Shift.WorkingHours,
            assignment.Shift.GraceMinutes);

        if (attendance is null)
        {
            attendance = new AttendanceRecord
            {
                EmployeeId = employeeId,
                WorkDate = workDate,
                CheckInAt = now,
                LateMinutes = late.Minutes
            };

            await _context.AttendanceRecords
                .AddAsync(attendance, cancellationToken);
        }
        else
        {
            attendance.CheckInAt = now;
            attendance.LateMinutes = late.Minutes;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(attendance);
    }

    public async Task<AttendanceDailyDto> CheckOut(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var workDate = WorkTimeCalculator.GetWorkDate(
            now, _companyTimeZone);

        var attendance = await _context.AttendanceRecords
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId &&
                x.WorkDate == workDate, cancellationToken);

        if (attendance is null || !attendance.CheckInAt.HasValue)
        {
            throw new BusinessRuleException(
                "Employee has not checked in.");
        }

        if (attendance.CheckOutAt.HasValue)
        {
            throw new ConflictException(
                "Employee has already checked out");
        }

        var assignment = await GetCurrentAssignment(
            employeeId,
            workDate, cancellationToken);

        var worked = WorkTimeCalculator.CalculateWorkedTime(
            attendance.CheckInAt.Value, now);

        var earlyLeave = WorkTimeCalculator.CalculateEarlyLeave(
            now, assignment.Shift.WorkingHours,
            _companyTimeZone);

        var overtime = WorkTimeCalculator.CalculateOvertime(
            worked, assignment.Shift.WorkingHours.Duration);

        attendance.CheckOutAt = now;
        attendance.WorkedMinutes = worked.Minutes;
        attendance.EarlyLeaveMinutes = earlyLeave.Minutes;
        attendance.OvertimeMinutes = overtime.Minutes;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(attendance);
    }

    public async Task<AttendanceDailyDto?> GetDaily(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var today = WorkTimeCalculator.GetWorkDate(
            DateTimeOffset.UtcNow, _companyTimeZone);

        var attendance = await _context.AttendanceRecords
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.EmployeeId == employeeId &&
                x.WorkDate == today, cancellationToken);

        return attendance is null ?
            null :
            MapToDto(attendance);
    }

    public async Task<PagedResultDto<AttendanceDailyDto>> GetEmployeeAttendance(
        int employeeId,
        AttendanceListRequestDto request,
        CancellationToken cancellationToken)
    {
        var query = _context.AttendanceRecords
            .AsNoTracking()
            .Where(x => x.EmployeeId == employeeId);

        if (request.From.HasValue) 
        {
            query = query.Where(x => x.WorkDate >= request.From.Value);
        }

        if (request.To.HasValue) 
        { 
            query = query.Where(x => x.WorkDate <= request.To.Value);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.WorkDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize).Select(x => new AttendanceDailyDto 
            { 
                Id = x.Id, 
                EmployeeId = x.EmployeeId,
                WorkDate = x.WorkDate,
                CheckInAt = x.CheckInAt, 
                CheckOutAt = x.CheckOutAt,
                WorkedMinutes = x.WorkedMinutes,
                LateMinutes = x.LateMinutes,
                EarlyLeaveMinutes = x.EarlyLeaveMinutes,
                OvertimeMinutes = x.OvertimeMinutes
            })
            .ToListAsync(cancellationToken);

        return new PagedResultDto<AttendanceDailyDto>
        {
            Items = items,
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
    private static AttendanceDailyDto MapToDto(AttendanceRecord attendance)
    {
        return new AttendanceDailyDto
        {
            Id = attendance.Id,
            EmployeeId = attendance.EmployeeId,
            WorkDate = attendance.WorkDate,
            CheckInAt = attendance.CheckInAt,
            CheckOutAt = attendance.CheckOutAt,
            WorkedMinutes = attendance.WorkedMinutes,
            LateMinutes = attendance.LateMinutes,
            EarlyLeaveMinutes = attendance.EarlyLeaveMinutes,
            OvertimeMinutes = attendance.OvertimeMinutes
        };
    }
    private async Task<EmployeeShiftAssignment> GetCurrentAssignment(
        int employeeId,
        DateOnly workDate,
        CancellationToken cancellation)
    {
        var assignment = await _context.ShiftAssignments
            .Include(x => x.Shift)
            .FirstOrDefaultAsync(x => x.EmployeeId == employeeId &&
            x.EffectiveFrom <= workDate &&
            (x.EffectiveTo == null || x.EffectiveTo >= workDate)
            , cancellation);

        if (assignment is null) 
        {
            throw new BusinessRuleException(
                "Employee does not have an active shift."); 
        }
        return assignment;
    }
}
