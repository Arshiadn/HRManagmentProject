using Azure.Core;
using HrApi.DTOs.Shifts;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Repositories;
using HrApi.Responses;
using HrApi.ValueObjects;
using HrApi.DTOs.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public class ShiftService : IShiftService
{
    private readonly HrDbContext _context;

    public ShiftService(
        HrDbContext context)
    {
        _context = context;
    }

    public async Task<ShiftDetailsDto> Create(
        CreateShiftDto request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        var shiftExists = await _context.Shifts
            .AnyAsync(x => x.Name == name, cancellationToken);

        if (shiftExists)
        {
            throw new ConflictException(
                "A shift with this name already exists.");
        }
        if(request.GraceMinutes < 0)
        {
            throw new BadRequestException(
                "Grace Minutes should not be negative");
        }

        var WorkingHours = TimeRange.Create(
            request.WorkingHours.Start,
            request.WorkingHours.End);

        var shift = new Shift
        {
            Name = request.Name,
            WorkingHours = WorkingHours,
            GraceMinutes = request.GraceMinutes,
            IsActive = request.IsActive
        };

        await _context.Shifts.AddAsync(shift, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return new ShiftDetailsDto
        {
            Id = shift.Id,
            Name = shift.Name,
            WorkingHours = new TimeRangeDto
            {
                Start = shift.WorkingHours.Start,
                End = shift.WorkingHours.End
            },
            GraceMinutes = shift.GraceMinutes,
            IsActive = shift.IsActive
        };
    }

    public async Task Update(
        int id,
        UpdateShiftDto request,
        CancellationToken cancellationToken)
    {
        var shift = await _context.Shifts
            .FirstOrDefaultAsync(
                x => x.Id == id, cancellationToken);

        if (shift is null)
        {
            throw new NotFoundException($"There no shift with {id} ID");
        }

        var nameExists = await _context.Shifts
            .AnyAsync(x => x.Id != id &&
                x.Name == request.Name,
                cancellationToken);

        if (nameExists)
        {
            throw new ConflictException(
                "A shift with this name already exists."); 
        }

        var WorkingHours = TimeRange.Create(
            request.WorkingHours.Start,
            request.WorkingHours.End);

        shift.Name = request.Name;
        shift.WorkingHours = WorkingHours;
        shift.GraceMinutes = request.GraceMinutes;
        shift.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
