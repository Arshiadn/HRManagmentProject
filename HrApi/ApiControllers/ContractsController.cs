using HrApi.DTOs.Contracts;
using HrApi.DTOs.Paging;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace HrApi.ApiControllers;

[Route("api/contracts")]
[ApiController]
public class ContractsController : ControllerBase
{
    private readonly IContractService _contractService;
    public ContractsController(IContractService contractService) => _contractService = contractService;

    [HttpGet]
    public async Task<ActionResult<PagedResultDto<ContractListItemDto>>>
        GetList(
        [FromQuery] ContractListRequest request,
        DateOnly today,
        CancellationToken cancellationToken)
    {
        var result =
            await _contractService.GetListAsync(
                request,
                today,
                cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ContractDetailsDto>>>
        GetById(
        int id,
        CancellationToken cancellation)
    {
        var result = await
            _contractService.GetByIdAsync(id, cancellation);

        return Ok(result);
    }
    [HttpPost]
    public async Task<IActionResult> Create(
        CreateContractRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _contractService
            .CreateContractAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }
    [HttpPost("{id}/submit-for-signature")]
    public async Task<IActionResult> SubmitSignature(
        int contractId,
        SubmitSignatureRequest request,
        CancellationToken cancellationToken)
    {
        await _contractService.SubmitSignatureAsync(
            contractId, request, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Activate(
        int contractId,
        ActivateContractRequest request,
        CancellationToken cancellationToken)
    {
        await _contractService.ActivateContractAsync(
            contractId, request, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id}/cancel")]
    public async Task<IActionResult> Cancel(
        int contractId, 
        CancelContractRequest request,
        CancellationToken cancellationToken)
    {
        await _contractService.CancelContractAsync(
            contractId, request, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id}/renew")]
    public async Task<IActionResult> Renew(
        int contractId,
        string reason,
        CancellationToken cancellationToken)
    {
        await _contractService.RenewContractAsync(
            contractId, reason, cancellationToken);

        return NoContent();
    }
}
