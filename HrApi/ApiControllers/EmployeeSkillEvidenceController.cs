using HrApi.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HrApi.DTOs.Skill.SkillEvidences;
using HrApi.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HrApi.ApiControllers;

[Route("api/skill-evidences")]
[ApiController]
[Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmployeeSkillEvidenceController : ControllerBase
{
    private readonly ISkillEvidenceService _evidenceService;
    public EmployeeSkillEvidenceController(ISkillEvidenceService evidenceService) 
        => _evidenceService = evidenceService;

    [Authorize(Roles = "Employee")]
    [HttpPost("employee/me/list")]
    public async Task<ActionResult<ApiResponse<SkillEvidenceDto>>> Create(
        CreateSkillEvidenceDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _evidenceService.CreateAsync(
            dto,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<SkillEvidenceDto>
            {
                Success = true,
                Message = "Skill evidence created successfully.",
                Data = result
            }
        );
    }
    [Authorize(Roles = "Employee")]
    [HttpGet("employee/me")]
    public async Task<ActionResult<ApiResponse<List<SkillEvidenceDto>>>> GetMine(
        CancellationToken cancellationToken)
    {
        var result = await _evidenceService.GetMineAsync(
            cancellationToken);

        return Ok(new ApiResponse<List<SkillEvidenceDto>>
            {
                Success = true,
                Message = "Skill evidence retrieved successfully.",
                Data = result
            });
    }
    [Authorize(Roles = "Admin,HRManager")]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<List<SkillEvidenceDto>>>> GetByEmployee(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var result = await _evidenceService.GetByEmployeeIdAsync(
            employeeId,
            cancellationToken);

        return Ok(new ApiResponse<List<SkillEvidenceDto>>
        {
            Success = true,
            Message = "Employee skill evidences retrieved successfully.",
            Data = result
        });
    }
    [Authorize(Roles = "Admin,HRManager")]
    [HttpPut("{evidenceId:long}")]
    public async Task<ActionResult<ApiResponse<SkillEvidenceDto>>> Update(
        long evidenceId,
        UpdateSkillEvidenceDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _evidenceService.UpdateAsync(
            evidenceId,
            dto,
            cancellationToken);

        return Ok(new ApiResponse<SkillEvidenceDto>
        {
            Success = true,
            Message = "Skill evidence updated successfully.",
            Data = result
        }); 
    }
}

