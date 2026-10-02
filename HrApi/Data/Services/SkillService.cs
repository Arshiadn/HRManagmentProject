using System;
using HrApi.DTOs.Skill.Skills;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models.Skill;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class SkillService : ISkillService
{
    private readonly HrDbContext _context;
    
    public SkillService(HrDbContext context) => _context = context;

    private static SkillDetailsDto MapToDto(Skill skill)
    {
        return new SkillDetailsDto
        {
          Id = skill.Id,
          Title = skill.Title,
          Description = skill.Description,
          IsActive = skill.IsActive  
        };
    }

    public async Task<SkillDetailsDto> CreateAsync(CreateSkillDto dto, CancellationToken cancellationToken)
    {
        var exists = await _context.Skills
            .AnyAsync(x => x.Title == dto.Title,
            cancellationToken);

        if (exists)
        {
            throw new NotFoundException("Skill with this title already exists.");
        } 
        var skill = new Skill
        {
          Title = dto.Title,
          Description = dto.Description,
          IsActive = true  
        };

        _context.Skills.Add(skill);

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(skill);
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (skill is null)
        {
            throw new NotFoundException(
                "Skill was not found.");
        }

        skill.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<SkillDetailsDto>> GetAllAsync(CancellationToken cancellationToken)
    {
            return await _context.Skills
            .AsNoTracking()
            .Select(x => new SkillDetailsDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                IsActive = x.IsActive
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<SkillDetailsDto> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (skill is null)
        {
            throw new NotFoundException(
                "Skill was not found.");
        }

        return MapToDto(skill);
    }

    public async Task<SkillDetailsDto> UpdateAsync(int id, UpdateSkillDto dto, CancellationToken cancellationToken)
    {
        var skill = await _context.Skills
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);

        if (skill is null)
        {
            throw new NotFoundException(
                "Skill was not found.");
        }

        var duplicateName = await _context.Skills
            .AnyAsync(
                x => x.Id != id &&
                x.Title == dto.Title,
                cancellationToken);

        if (duplicateName)
        {
            throw new BusinessRuleException(
                "A skill with this name already exists.");
        }

        skill.Title = dto.Title;
        skill.Description = dto.Description;
        skill.IsActive = dto.IsActive;

        await _context.SaveChangesAsync(cancellationToken);

        return MapToDto(skill);
    }
}
