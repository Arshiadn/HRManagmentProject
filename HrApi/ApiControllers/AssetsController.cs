using HrApi.DTOs.Assets;
using HrApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/assets")]
[ApiController]
public sealed class AssetsController : ControllerBase
{
    private readonly IAssetService _assetService;

    public AssetsController(
        IAssetService assetService)
    {
        _assetService = assetService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreateAssetDto dto,
        CancellationToken cancellationToken)
    {
        var id = await _assetService.CreateAsync(
            dto,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _assetService.GetAllAsync(
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(result);
    }

    [HttpPost("{id:int}/assign")]
    public async Task<IActionResult> Assign(
        int id,
        int employeeId,
        string? note,
        CancellationToken cancellationToken)
    {
        await _assetService.AssignAsync(
            id,
            employeeId,
            note,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/return")]
    public async Task<IActionResult> Return(
        int id,
        string? note,
        CancellationToken cancellationToken)
    {
        await _assetService.ReturnAsync(
            id,
            note,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/send-to-repair")]
    public async Task<IActionResult> SendToRepair(
        int id,
        CancellationToken cancellationToken)
    {
        await _assetService.SendToRepairAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/complete-repair")]
    public async Task<IActionResult> CompleteRepair(
        int id,
        CancellationToken cancellationToken)
    {
        await _assetService.CompleteRepairAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/report-lost")]
    public async Task<IActionResult> ReportLost(
        int id,
        CancellationToken cancellationToken)
    {
        await _assetService.ReportLostAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:int}/report-damaged")]
    public async Task<IActionResult> ReportDamaged(
        int id,
        CancellationToken cancellationToken)
    {
        await _assetService.ReportDamagedAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpGet("{id:int}/history")]
    public async Task<IActionResult> GetHistory(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _assetService.GetHistoryAsync(
            id,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("inventory-summary")]
    public async Task<IActionResult> GetInventorySummary(
        CancellationToken cancellationToken)
    {
        var result =
            await _assetService.GetInventorySummaryAsync(
                cancellationToken);

        return Ok(result);
    }
}
