using HrApi.DTOs.Attendance;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/attendance")]
[ApiController]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceService _attendanceService;
    public AttendanceController(IAttendanceService attendanceService) => _attendanceService = attendanceService;
    [HttpPost("check-in")]
    public async Task<ActionResult<ApiResponse<AttendanceDailyDto>>> 
        CheckIn(
        int employeeId,
        CancellationToken cancellationToken)
    {
         var result = await _attendanceService
            .CheckIn(employeeId, cancellationToken);

        return Ok(new ApiResponse<AttendanceDailyDto>
        {
            Success = true,
            Message = "Check-in successful",
            Data = result
        });
    }
    [HttpPost("check-out")]
    public async Task<ActionResult<ApiResponse<AttendanceDailyDto>>> 
        CheckOut(
        int employeeId,
        CancellationToken cancellationToken)
    {
         var result = await _attendanceService
            .CheckOut(employeeId, cancellationToken);

        return Ok(new ApiResponse<AttendanceDailyDto>
        {
            Success = true,
            Message = "Check-out successful",
            Data = result
        });
    }
    [HttpGet("daily")]
    public async Task<ActionResult<ApiResponse<AttendanceDailyDto>>> 
        GetDailyAttendance(
        int employeeId,
        CancellationToken cancellationToken)
    {
         var result = await _attendanceService
            .GetDaily(employeeId, cancellationToken);

        if (result is null)
        {
            throw new BadRequestException(
                "There no attendance record for the employee today");
        }

        return Ok(new ApiResponse<AttendanceDailyDto>
        {
            Success = true,
            Message = "Daily attendance retrieved successfully",
            Data = result
        });
    }
}
