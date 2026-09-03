using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HrApi.Interfaces;
using HrApi.Responses;
using HrApi.DTOs.Shifts;

namespace HrApi.ApiControllers;

[Route("api/shifts")]
[ApiController]
public class ShiftsController : ControllerBase
{
    private readonly IShiftService _shiftService;
    public ShiftsController(IShiftService shiftService) => _shiftService = shiftService;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<ShiftDetailsDto>>> Create(
        CreateShiftDto request, 
        CancellationToken cancellation)
    {
        var shift = await _shiftService.Create(request, cancellation);

        return StatusCode(
            StatusCodes.Status201Created,
            new ApiResponse<ShiftDetailsDto>
            { 
                Success = true,
                Message = $"Shift Created with ID {shift.Id}",
                Data = shift
            });
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateShiftDto request,
        CancellationToken cancellation)
    {
        await _shiftService.Update(id, request, cancellation); 

        return NoContent();
    }
}
