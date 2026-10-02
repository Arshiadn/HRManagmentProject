using HrApi.DTOs.Skill.SkillMatrix;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SkillMatrixController : ControllerBase
{
    private readonly ISkillMatrixService _skillMatrixService;

    public SkillMatrixController(ISkillMatrixService skillMatrixService) 
        => _skillMatrixService = skillMatrixService;

    [Authorize(Roles = "Employee")]
    [HttpGet("employee/me")]
    public async Task<ActionResult<ApiResponse<SkillMatrixDto>>> GetMine(
        CancellationToken cancellationToken)
    {
        var result = await _skillMatrixService.GetMineAsync(cancellationToken);

        return Ok(new ApiResponse<SkillMatrixDto>
        {
            Success = true,
            Message = "Skill matrix retrieved successfully.",
            Data = result
        });
    }
    [Authorize(Roles = "Admin,HRManager")] 
    [HttpGet("employees/{employeeId:int}")]
    public async Task<ActionResult<ApiResponse<SkillMatrixDto>>> GetByEmployeeId(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var result = await _skillMatrixService.GetByEmployeeIdAsync(employeeId, cancellationToken);

        return Ok(new ApiResponse<SkillMatrixDto>
        {
            Success = true,
            Message = "Employee skill matrix retrieved successfully.",
            Data = result
        });
    }
    [Authorize(Roles = "Admin,HRManager")]
    [HttpGet("department/{departmentId:int}")]
    public async Task<ActionResult<ApiResponse<List<SkillMatrixDto>>>> GetByDepartment(
        int departmentId,
        CancellationToken cancellationToken)
    {
        var result = await _skillMatrixService
            .GetByDepartmentIdAsync(
                departmentId,
                cancellationToken);

        return Ok(new ApiResponse<List<SkillMatrixDto>>
        {
            Success = true,
            Message = "Department skill matrix retrieved successfully.",
            Data = result
        });
    }
}

