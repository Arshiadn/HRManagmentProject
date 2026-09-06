using HrApi.DTOs.EmployeeRequests;
using HrApi.DTOs.Paging;
using HrApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/employee-requests")]
[ApiController]
public sealed class EmployeeRequestsController : ControllerBase
{
    private readonly IEmployeeRequestService _service;

    public EmployeeRequestsController(
        IEmployeeRequestService service)
    {
        _service = service;
    }
    [HttpPost]
    public async Task<ActionResult<EmployeeRequestDetailsDto>> Create(
           CreateEmployeeRequestDto dto,
           CancellationToken cancellationToken)
    {
        var result = await _service.CreateAsync(
            dto,
            cancellationToken);

        return Created(
            $"/api/employee-requests/{result.Id}",
            result);
    }
    [HttpPost("{id:long}/submit")]
    public async Task<IActionResult> Submit(
        long id,
        CancellationToken cancellationToken)
    {
        await _service.SubmitAsync(
            id,
            cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:long}/approve")]
    public async Task<IActionResult> Approve(
        long id,
        CancellationToken cancellationToken)
    {
        await _service.ApproveAsync(
            id,
            cancellationToken);

        return NoContent();
    }

    [HttpPost("{id:long}/reject")]
    public async Task<IActionResult> Reject(
        long id,
        CancellationToken cancellationToken)
    {
        await _service.RejectAsync(
            id,
            cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:long}/cancel")]
    public async Task<IActionResult> Cancel(
        long id,
        CancellationToken cancellationToken)
    {
        await _service.CancelAsync(
            id,
            cancellationToken);

        return NoContent();
    }
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<EmployeeRequestDetailsDto>>> 
        GetList(
        [FromQuery] EmployeeRequestListRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _service.GetListAsync(
            request,
            cancellationToken);

        return Ok(result);
    }
}
