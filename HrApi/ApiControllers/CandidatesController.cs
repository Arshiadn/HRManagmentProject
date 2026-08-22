using HrApi.DTOs.Candidates;
using HrApi.DTOs.Interviews;
using HrApi.DTOs.Paging;
using HrApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace HrApi.ApiControllers;

[Route("api/candidate")]
[ApiController]
public sealed class CandidatesController : ControllerBase
{
    private readonly IRecruitmentService _recruitmentService;
    private readonly ICandidateServicecs _candidateServicecs;
    public CandidatesController(
        IRecruitmentService recruitmentService,
        ICandidateServicecs candidateService)
    {
        _recruitmentService = recruitmentService;
        _candidateServicecs = candidateService;
    }

    [HttpPost]
    public async Task<ActionResult> Create(
        CreateCandidateRequest request,
        CancellationToken cancellationToken)
    {
        var id = await _recruitmentService.CreateCandidateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _candidateServicecs.GetByIdAsync(id, cancellationToken);

        return Ok(result);
    }
    [HttpPost("{id:int}/schedule-interview")]
    public async Task<IActionResult> ScheduleInterview
        (int id,
        ScheduleInterviewRequest request,
        CancellationToken cancellationToken)
    {
        await _recruitmentService.ScheduleInterviewAsync(id, request, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:int}/complete-interview")]
    public async Task<IActionResult> CompleteInterView
        (int candidateId,
        CompleteInterviewRequest request,
        CancellationToken cancellationToken)
    {
        await _recruitmentService.CompleteInterviewAsync(candidateId, request, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> Accept(
    int id,
    CancellationToken cancellationToken)
    {
        await _recruitmentService
            .AcceptCandidateAsync(
                id,
                cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> Reject(
    int id,
    RejectCandidateRequest request,
    CancellationToken cancellationToken)
    {
        await _recruitmentService
            .RejectCandidateAsync(
                id,
                request,
                cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:int}/hire")]
    public async Task<ActionResult> Hire(
    int id,
    HireCandidateRequest request,
    CancellationToken cancellationToken)
    {
        var employeeId =
            await _recruitmentService
                .HireCandidateAsync(
                    id,
                    request,
                    cancellationToken);

        return Ok(new { employeeId });
    }
    [HttpGet("list")]
    public async Task<ActionResult<PagedResultDto<CandidateDetailsDto>>>
        GetListAsync([FromQuery]CandidateListRequest request, CancellationToken cancellationToken)
    {
        var result = await _candidateServicecs.GetListAsync(request, cancellationToken);

        return Ok(result);
    }
    [HttpPost("{id:int}/reopen")]
    public async Task<IActionResult> Reopen(
        int candidateId,
        CancellationToken cancellationToken)
    {
        await _recruitmentService.ReopenCandidateAsync(candidateId, cancellationToken);

        return NoContent();
    }
}
