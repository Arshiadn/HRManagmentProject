using HrApi.DTOs.Positions;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HrApi.DTOs.Skill.PositionSkills;

namespace HrApi.ApiControllers;

    [Route("api/positions")]
    [ApiController]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Admin,HRManager")]
public class PositionController : ControllerBase
{
    private readonly IPositionService _positionService;
    private readonly IPositionSkillService _positionSkillService;
    public PositionController(IPositionService positionService, IPositionSkillService positionSkillService)
    {
        _positionService = positionService;
        _positionSkillService = positionSkillService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePositionDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _positionService.CreateAsync(dto, cancellationToken);

        return CreatedAtAction(
            nameof(Create),
            new {Id = result.Id},
            result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(
        CancellationToken cancellationToken)
    {
        var result = await _positionService.GetAllAsync(cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _positionService.GetByIdAsync(
            id,
            cancellationToken);

        if (result == null)
        {
            throw new NotFoundException($"Position with ID {id} not found.");
        }

        return Ok(result);
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdatePositionDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _positionService.UpdateAsync(
            id,
            dto,
            cancellationToken);

        if (result == null)
        {
            throw new NotFoundException($"Position with ID {id} not found.");
        }

        return Ok(result);
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _positionService.DeleteAsync(
            id,
            cancellationToken);

        if (!result)
        {
            throw new NotFoundException($"Position with ID {id} not found.");
        }

        return NoContent();
    }
    [HttpPost("{positionId:int}/skills")]
    public async Task<ActionResult<ApiResponse<PositionSkillDto>>> AddSkill(
        int positionId,
        CreatePositionSkillDto dto,
        CancellationToken cancellationToken)
    {
        var result = await _positionSkillService.CreateAsync(
            positionId,
            dto,
            cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<PositionSkillDto>
            {
                Success = true,
                Message = "Position skill created successfully.",
                Data = result
            }
        );
    }
    [HttpGet("{positionId:int}/skills")]
    public async Task<ActionResult<ApiResponse<List<PositionSkillDto>>>> GetSkills(
        int positionId,
        CancellationToken cancellationToken)
    {
        var result = await _positionSkillService.GetAllAsync(
            positionId,
            cancellationToken);

        return Ok(
            new ApiResponse<List<PositionSkillDto>>
            {
                Success = true,
                Message = "Position skill retrieved successfully.",
                Data = result
            }
        );
    }
    [HttpPut("{positionId:int}/skills/{skillId:int}")]
    public async Task<IActionResult> UpdateSkill(
        int positionId,
        int skillId,
        UpdatePositionSkillDto dto,
        CancellationToken cancellationToken)
    {
        await _positionSkillService.UpdateAsync(positionId, skillId, dto, cancellationToken);

        return NoContent();
    }
    [HttpDelete("{positionId:int}/skills/{skillId:int}")]
    public async Task<IActionResult> DeleteSkill(
        int positionId,
        int skillId,
        CancellationToken cancellationToken)
    {
        await _positionSkillService.DeleteAsync(
            positionId,
            skillId,
            cancellationToken);

        return NoContent();
    }
    [HttpGet("{positionId}/skills/{skillId}/history")]
    public async Task<ActionResult<ApiResponse<List<PositionSkillHistoryDto>>>> GetPositionSkillHistory(
        int positionId,
        int skillId,
        CancellationToken cancellationToken)
    {
        var result = await _positionSkillService.GetHistoryAsync(
            positionId,
            skillId,
            cancellationToken);

        return Ok( new ApiResponse<List<PositionSkillHistoryDto>>
        {
            Success = true,
            Message = "Position Skill History retrieved",
            Data = result
        });
    }
}