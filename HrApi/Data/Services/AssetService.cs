using HrApi.DTOs.Assets;
using HrApi.DTOs.Assets.Assignment;
using HrApi.Enums.Asset;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class AssetService : IAssetService
{
    private readonly HrDbContext _context;
    public AssetService(HrDbContext context) => _context = context;

    public async Task<int> CreateAsync(
        CreateAssetDto dto,
        CancellationToken cancellationToken)
    {
        var assetExists = await _context.CompanyAssets
            .AnyAsync(x => x.AssetCode == dto.AssetCode,
            cancellationToken);

        if (assetExists)
            throw new ConflictException(
                "Asset Already Exists.");

        var asset = new CompanyAsset
        {
            AssetCode = dto.AssetCode,
            Title = dto.Title,
            SerialNumber = dto.SerialNumber,
            Type = dto.Type
        };

        _context.CompanyAssets.Add(asset);

        await SaveChangesAsync(cancellationToken);

        return asset.Id;
    }
    public async Task<IReadOnlyList<AssetListDto>> GetAllAsync(
        CancellationToken cancellationToken)
    {
        return await _context.CompanyAssets
            .AsNoTracking()
            .Select(x => new AssetListDto
            {
                Id = x.Id,
                AssetCode = x.AssetCode,
                Title = x.Title,
                Type = x.Type,
                Status = x.Status
            })
            .ToListAsync(cancellationToken);
    }
    public async Task<AssetDetailsDto> GetByIdAsync(
        int assetId,
        CancellationToken cancellationToken)
    {
        return await _context.CompanyAssets
            .AsNoTracking()
            .Where(x => x.Id == assetId)
            .Select(x => new AssetDetailsDto
            {
                Id = x.Id,
                AssetCode = x.AssetCode,
                Title = x.Title,
                SerialNumber = x.SerialNumber,
                Type = x.Type,
                Status = x.Status
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(
                "Asset not found.");
    }
    public async Task AssignAsync(
        int assetId,
        int employeeId,
        string? note,
        CancellationToken cancellationToken)
    {
        var asset = await _context.CompanyAssets
            .FirstOrDefaultAsync(x => x.Id == assetId,
            cancellationToken)
            ?? throw new NotFoundException(
                "asset not found.");

        if (asset.Status != AssetStatus.Available)
            throw new BusinessRuleException(
                "Asset is not available.");

        var employeeExists = await _context.Employees
            .AnyAsync(
            x => x.Id == employeeId,
            cancellationToken);

        if(!employeeExists)
            throw new NotFoundException(
            "Employee not found.");

        var assignment = new AssetAssignment
        {
            AssetId = asset.Id,
            EmployeeId = employeeId,
            AssignedAt = DateTimeOffset.UtcNow,
            AssignmentNote = note
        };

        _context.AssetAssignments.Add(assignment);

        asset.MarkAsAssigned();

        await SaveChangesAsync(cancellationToken);
    }

    public async Task ReturnAsync(
        int assetId,
        string? note,
        CancellationToken cancellationToken)
    {
        var asset = await _context.CompanyAssets
            .FirstOrDefaultAsync(x =>
            x.Id == assetId, cancellationToken)
            ?? throw new NotFoundException("Asset not found");

        var assignment = await _context.AssetAssignments
            .Where(x => x.AssetId == assetId && x.ReturnedAt == null)
            .OrderByDescending(x => x.AssignedAt)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Active assignment not found");

        assignment.ReturnedAt = DateTimeOffset.UtcNow;

        assignment.ReturnNote = note;

        asset.MarkAsAvailable();

        await SaveChangesAsync(cancellationToken);
    }

    public async Task SendToRepairAsync(
        int assetId,
        CancellationToken cancellationToken)
    {
        var asset = await _context.CompanyAssets
            .FirstOrDefaultAsync(
                x => x.Id == assetId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Asset not found.");

        asset.SendToRepair();

        await SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteRepairAsync(
        int assetId,
        CancellationToken cancellationToken)
    {
        var asset = await _context.CompanyAssets
            .FirstOrDefaultAsync(
                x => x.Id == assetId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Asset not found.");

        asset.CompleteRepair();

        await SaveChangesAsync(cancellationToken);
    }

    public async Task ReportLostAsync(
        int assetId,
        CancellationToken cancellationToken)
    {
        var asset = await _context.CompanyAssets
            .FirstOrDefaultAsync(
                x => x.Id == assetId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Asset not found.");

        asset.MarkAsLost();

        await SaveChangesAsync(cancellationToken);
    }

    public async Task ReportDamagedAsync(
        int assetId,
        CancellationToken cancellationToken)
    {
        var asset = await _context.CompanyAssets
            .FirstOrDefaultAsync(
                x => x.Id == assetId,
                cancellationToken)
            ?? throw new NotFoundException(
                "Asset not found.");

        asset.MarkAsDamaged();

        await SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<AssetAssignmentHistoryDto>>
        GetHistoryAsync(
        int assetId, CancellationToken cancellationToken)
    {
        var assetExists = await _context.CompanyAssets
        .AnyAsync(
            x => x.Id == assetId,
            cancellationToken);

        if (!assetExists)
            throw new NotFoundException(
                "Asset not found.");

        return await _context.AssetAssignments
            .AsNoTracking()
            .Where(x => x.AssetId == assetId)
            .OrderByDescending(x => x.AssignedAt)
            .Select(x => new AssetAssignmentHistoryDto
            {
                AssignmentId = x.Id,

                AssetId = x.AssetId,

                EmployeeId = x.EmployeeId,

                AssignedAt = x.AssignedAt,

                ReturnedAt = x.ReturnedAt,

                AssignmentNote = x.AssignmentNote,

                ReturnNote = x.ReturnNote
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<InventoryItemDto>> 
        GetInventorySummaryAsync(CancellationToken cancellationToken)
    {
        return await _context.CompanyAssets
            .AsNoTracking()
            .GroupBy(x => x.Status)
            .Select(g => new InventoryItemDto
            {
                Status = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<EmployeeAssetDto>> 
        GetEmployeeAssetsAsync(
        int employeeId, CancellationToken cancellationToken)
    {
        var employeeExists = await _context.Employees
        .AnyAsync(
            x => x.Id == employeeId,
            cancellationToken);

        if (!employeeExists)
            throw new NotFoundException(
                "Employee not found.");

        return await _context.AssetAssignments
        .AsNoTracking()
        .Where(x =>
            x.EmployeeId == employeeId &&
            x.ReturnedAt == null)
        .Select(x => new EmployeeAssetDto
        {
            AssetId = x.AssetId,
            AssignedAt = x.AssignedAt
        })
        .ToListAsync(cancellationToken);
    }
    private async Task SaveChangesAsync(
    CancellationToken cancellationToken)
    {
        try
        {
            await _context.SaveChangesAsync(
                cancellationToken);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "Asset was changed by another operation. " +
                "Reload the asset and try again.");
        }
    }
}
