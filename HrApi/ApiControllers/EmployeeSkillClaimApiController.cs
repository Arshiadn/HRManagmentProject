using HrApi.DTOs.Skill.EmployeeSkillClaim;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[ApiController]
[Route("api/skill-claims")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmployeeSkillClaimApiController : ControllerBase
{
    private readonly IEmployeeSkillClaimService _claimService;

    public EmployeeSkillClaimApiController(
        IEmployeeSkillClaimService claimService)
    {
        _claimService = claimService;
    }

    [HttpPost("employee/me")]
    [Authorize(Roles = "Employee")]
    public async Task<ActionResult<ApiResponse<EmployeeSkillClaimDto>>> Create(
        CreateEmployeeSkillClaimDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _claimService.CreateAsync(
            dto,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<EmployeeSkillClaimDto>
            {
                Success = true,
                Message = "Skill claim created successfully.",
                Data = result
            });
    }
    [Authorize(Roles = "Admin,HRManager")]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<List<EmployeeSkillClaimDto>>>> GetByEmployee(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var result = await _claimService.GetByEmployeeIdAsync(
            employeeId,
            cancellationToken);

        return Ok(new ApiResponse<List<EmployeeSkillClaimDto>>
        {
            Success = true,
            Message = "Employee skill claims retrieved successfully.",
            Data = result
        });
    }
}

