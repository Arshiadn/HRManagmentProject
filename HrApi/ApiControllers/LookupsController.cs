using HrApi.DTOs.Lookup;
using HrApi.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[ApiController]
[Route("api/lookups")]
public class LookupsController : ControllerBase
{
    private readonly ILookupService _lookupService;
    public LookupsController(ILookupService lookupService) => _lookupService = lookupService;

    [HttpGet("departments")]
    public async Task<ActionResult<
    IReadOnlyList<LookupItemDto>>> GetDepartments(
        CancellationToken cancellationToken)
    {
        var result = await _lookupService
                .GetActiveDepartmentsAsync(cancellationToken);

        return Ok(result);
    }
}
