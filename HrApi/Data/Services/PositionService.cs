using System;
using HrApi.Interfaces;
using HrApi.DTOs.Positions;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;
using HrApi.Exceptions;

namespace HrApi.Data.Services;

public class PositionService : IPositionService
{
    private readonly HrDbContext _context;

    public PositionService(HrDbContext dbContext)
    {
        _context = dbContext;
    }

    public async Task<PositionDetailsDto> CreateAsync(
        CreatePositionDto dto,
        CancellationToken cancellationToken)
    {
        var position = new Position
        {
            Title = dto.Title,
            Description = dto.Description,
            IsActive = dto.IsActive
        };

        _context.Positions.Add(position);

        await _context.SaveChangesAsync(cancellationToken);

        return new PositionDetailsDto
        {
            Id = position.Id,
            Title = position.Title,
            Description = position.Description,
            IsActive = position.IsActive
        };
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var position = await _context.Positions
        .FirstOrDefaultAsync(
            p => p.Id == id,
            cancellationToken);

        if (position == null)
        {
            return false;
        }

        position.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<List<PositionDetailsDto>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.Positions
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Select(p => new PositionDetailsDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                IsActive = p.IsActive
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<PositionDetailsDto?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken)
    {
        return await _context.Positions
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(p => new PositionDetailsDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = p.Description,
                IsActive = p.IsActive
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
    public async Task<PositionDetailsDto?> UpdateAsync(
    int id,
    UpdatePositionDto dto,
    CancellationToken cancellationToken)
    {
        var position = await _context.Positions
        .FirstOrDefaultAsync(
            p => p.Id == id,
            cancellationToken);

        if (position == null)
        {
            throw new NotFoundException($"Position with ID {id} not found.");
        }

        position.Title = dto.Title;
        position.Description = dto.Description;
        position.IsActive = dto.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return new PositionDetailsDto
        {
            Id = position.Id,
            Title = position.Title,
            Description = position.Description,
            IsActive = position.IsActive
        };
    }
}
