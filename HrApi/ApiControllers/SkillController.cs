using HrApi.DTOs.Skill.Skills;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/skills")]
[ApiController]
public class SkillController : ControllerBase
{
    private readonly ISkillService _skillService;

    public SkillController(ISkillService skillService) => _skillService = skillService;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<SkillDetailsDto>>> Create(
        CreateSkillDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.CreateAsync(
            dto,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<SkillDetailsDto>{
            Success = true,
            Message = "Skill created successfully.",
            Data = result
        });
    }
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<SkillDetailsDto>>>> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetAllAsync(
            cancellationToken);
            
        return Ok(new ApiResponse<List<SkillDetailsDto>>
        {
            Success = true,
            Message = "Skills retrieved successfully.",
            Data = result
        });
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ApiResponse<SkillDetailsDto>>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(new ApiResponse<SkillDetailsDto>
        {
            Success = true,
            Message = "Skill retrieved successfully.",
            Data = result
        });
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse<SkillDetailsDto>>> Update(
        int id,
        UpdateSkillDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _skillService.UpdateAsync(
            id,
            dto,
            cancellationToken);

        return Ok(new ApiResponse<SkillDetailsDto>
        {
            Success = true,
            Message = "Skill updated successfully.",
            Data = result
        });
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _skillService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}

