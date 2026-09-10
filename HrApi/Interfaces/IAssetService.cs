using HrApi.DTOs.Assets;
using HrApi.DTOs.Assets.Assignment;

namespace HrApi.Interfaces;

public interface IAssetService
{
    Task<int> CreateAsync(
        CreateAssetDto dto,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AssetListDto>> GetAllAsync(
        CancellationToken cancellationToken);

    Task<AssetDetailsDto> GetByIdAsync(
        int assetId,
        CancellationToken cancellationToken);

    Task AssignAsync(
        int assetId,
        int employeeId,
        string? note,
        CancellationToken cancellationToken);

    Task ReturnAsync(
        int assetId,
        string? note,
        CancellationToken cancellationToken);

    Task SendToRepairAsync(
        int assetId,
        CancellationToken cancellationToken);

    Task CompleteRepairAsync(
        int assetId,
        CancellationToken cancellationToken);

    Task ReportLostAsync(
        int assetId,
        CancellationToken cancellationToken);

    Task ReportDamagedAsync(
        int assetId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<AssetAssignmentHistoryDto>> GetHistoryAsync(
        int assetId,
        CancellationToken cancellationToken);

    Task<IReadOnlyList<InventoryItemDto>> GetInventorySummaryAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyList<EmployeeAssetDto>> GetEmployeeAssetsAsync(
        int employeeId,
        CancellationToken cancellationToken);
}
