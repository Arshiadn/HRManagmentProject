using HrApi.DTOs.Skill.SkillAssessments;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/skill-assessments")]
[ApiController]
[Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SkillAssessmentApiController : ControllerBase
{
    private readonly ISkillAssessmentService _assessmentService;

    public SkillAssessmentApiController(ISkillAssessmentService assessmentService) 
        => _assessmentService = assessmentService;

    [HttpPost]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<ActionResult<ApiResponse<SkillAssessmentDto>>> Create(
    CreateSkillAssessmentDto dto, CancellationToken cancellationToken)
    {
        var result = await _assessmentService.CreateAsync(dto, cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<SkillAssessmentDto>
            {
                Success = true,
                Message = "Skill assessment created successfully.",
                Data = result
            });
    }
}
