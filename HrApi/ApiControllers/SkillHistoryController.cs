using System;
using HrApi.DTOs.Skill.History;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;


[Route("api/skill-history")]
[ApiController]
[Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public sealed class SkillHistoryController : ControllerBase
{
    private readonly ISkillStateHistoryService _historyService;

    public SkillHistoryController(
        ISkillStateHistoryService historyService)
    {
        _historyService = historyService;
    }

    [Authorize(Roles = "Admin,HRManager")]
    [HttpGet("employee/{employeeId:int}/skill/{skillId:int}")]
    public async Task<ActionResult<ApiResponse<List<SkillStateHistoryDto>>>> Get(
        int employeeId,
        int skillId,
        CancellationToken cancellationToken)
    {
        var result = await _historyService
            .GetByEmployeeAndSkillAsync(
                employeeId,
                skillId,
                cancellationToken);

        return Ok(new ApiResponse<List<SkillStateHistoryDto>>
        {
            Success = true,
            Message = "Employee skill history retrieved successfully.",
            Data = result
        });
    }
    [Authorize(Roles = "Admin,HRManager")]
    [HttpGet("employee/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<List<EmployeeSkillHistoryDto>>>> GetByEmployee(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var result = await _historyService.GetByEmployeeIdAsync(
            employeeId,
            cancellationToken);

        return Ok(new ApiResponse<List<EmployeeSkillHistoryDto>>
        {
            Success = true,
            Message = "Employee skill history retrieved successfully.",
            Data = result
        });
    }
}
