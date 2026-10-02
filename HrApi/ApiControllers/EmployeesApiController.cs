using AutoMapper;
using HrApi.DTOs.Attendance;
using HrApi.DTOs.Employees;
using HrApi.DTOs.Paging;
using HrApi.DTOs.Reviews.PerformanceReview;
using HrApi.DTOs.ShiftAssignment;
using HrApi.Interfaces;
using HrApi.Models;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HrApi.Controllers;

[ApiController]
[Route("api/employee")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    private readonly IAttendanceService _attendanceService;
    private readonly IEmployeeShiftAssignmentService _employeeShiftAssignmentService;
    private readonly IEmployeeRequestService _employeeRequestService;
    private readonly IAssetService _assetService;
    private readonly IReviewService _reviewService;
    public EmployeesApiController(
        IEmployeeService employeeService,
        IAttendanceService attendanceService,
        IEmployeeShiftAssignmentService employeeShiftAssignmentService,
        IEmployeeRequestService employeeRequestService,
        IAssetService assetService,
        IReviewService reviewService)
    {
        _employeeService = employeeService;
        _attendanceService = attendanceService;
        _employeeShiftAssignmentService = employeeShiftAssignmentService;
        _employeeRequestService = employeeRequestService;
        _assetService = assetService;
        _reviewService = reviewService;
    }
    [HttpGet]
    [Authorize(Roles = "Admin,HRManager")]
    public ActionResult<List<EmployeeListDto>> GetAll()
    {
        var result = _employeeService.GetAll();
        return Ok(result);
    }
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,HRManager")]
    public ActionResult<ApiResponse<EmployeeDetailsDto?>> GetById(int id)
    {
        var employee = _employeeService.GetById(id);

        if (employee == null)
        {
            return NotFound(new ApiErrorResponse
            {
                Message = "کارمند مورد نظر پیدا نشد"
            });
        }

        return Ok(new ApiResponse<EmployeeDetailsDto>
        {
            Success = true,
            Message = "اطلاعات کارمند دریافت شد",
            Data = employee
        });
    }
    [HttpPost]
    [Authorize(Roles = "Admin,HRManager")]
    public ActionResult<EmployeeDetailsDto> Create([FromBody] CreateEmployeeDto model)
    {
            var result = _employeeService.Create(model);
            return CreatedAtAction(
                nameof(GetById),
                new { id = result.Id },
                result);
    }
    //authorize by Hr Manager and admin
    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,HRManager")]
    public IActionResult Update(int id, UpdateEmployeeDto model)
    {
            _employeeService.Update(id, model);
            return NoContent();
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,HRManager")]
    public IActionResult Delete(int id)
    {
            _employeeService.Delete(id);
            return NoContent();
    }
    [HttpGet("search")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> Search([FromQuery]EmployeeSearchRequestDto request)
    {
        var result = await _employeeService.Search(request);
        return Ok(new ApiResponse<PagedResultDto<EmployeeListDto>>
        {
            Success = true,
            Message = "Employee search completed successfully",
            Data = result
        });
    }
    // update later: this should be only for employee role
    [HttpPost("{id:int}/photo")]
    [Consumes("multipart/form-data")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> UploadPhoto(int id, [FromForm]EmployeePhotoUploadDto model)
    {
            var result = await _employeeService.UploadPhotoAsync(id, model);
            return Ok(new ApiResponse<EmployeePhotoDto>
            {
                Success = true,
                Message = "تصویر کارمند ذخیره شد",
                Data = result
            });
    }
    [HttpGet("{id:int}/photo")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> GetPhoto(int id)
    {
            var result = await _employeeService.GetPhotoAsync(id);

            return Ok(new ApiResponse<EmployeePhotoDto>
            {
                Success = true,
                Message = "آدرس تصویر دریافت شد",
                Data = result
            });
    }
    [HttpGet("{id:int}/contract/download")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> DownloadContract(int id)
    {
            var file = await _employeeService.DownloadContractAsync(id);
            return File(
                file.Content,
                file.ContentType,
                file.DownloadName
            );
    }
    [HttpDelete("{id:int}/photo")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> DeletePhoto(int id)
    {
            await _employeeService.DeletePhotoAsync(id);
            return NoContent();
    }
    [HttpPut("{id}/personnel-code")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> AssignPersonnelCode(
    int id,
    string personnelCode,
    CancellationToken cancellationToken)
    {
        await _employeeService.AssignPersonnelCodeAsync(
            id,
            personnelCode,
            cancellationToken);

        return NoContent();
    }
    [HttpPut("transfer")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> 
        TransferEmployees(TransferEmployeesDto request,  CancellationToken cancellationToken)
    {
        await _employeeService.TransferEmployeesAsync(request, cancellationToken);

        return Ok(new ApiResponse<TransferEmployeesDto>
        {
            Success = true,
            Message = "Employees transferred successfully",
            Data = request
        });
    }
    [HttpGet("list")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<ActionResult<PagedResultDto<EmployeeListItemDto>>>
        GetListAsync([FromQuery] EmployeeListRequest request, CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetListAsync(request, cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id}/attendance")]
    [Authorize(Roles = "Admin,HRManager,Employee")]
    public async Task<ActionResult<PagedResultDto<AttendanceDailyDto>>> GetEmployeeAttendance(
        int id,
        [FromQuery] AttendanceListRequestDto request,
        CancellationToken cancellationToken)
    {
        var result = await _attendanceService
            .GetEmployeeAttendance(id, request, cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:int}/shift-assignments")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ShiftAssignmentDetailsDto>>>> 
        GetShiftAssignments(
            int employeeId,
            CancellationToken cancellationToken)
    {
        var assignment = await _employeeShiftAssignmentService
            .GetAssignmentsAsync(employeeId, cancellationToken);

        return Ok(new ApiResponse<IReadOnlyList<ShiftAssignmentDetailsDto>>
        {
            Success = true,
            Message = "Shift assignments retrieved successfully",
            Data = assignment
        });
    }
    [HttpPost("{id:int}/shift-assignments")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<ActionResult<ApiResponse<ShiftAssignmentDetailsDto>>> 
        CreateShiftAssignment(
            int employeeId,
            [FromBody] CreateShiftAssignmentDto request,
            CancellationToken cancellationToken)
    {
        var assignment = await _employeeShiftAssignmentService
            .AssignShiftAsync(employeeId, request, cancellationToken);

        return StatusCode(
        StatusCodes.Status201Created,
        new ApiResponse<ShiftAssignmentDetailsDto>
        {
            Success = true,
            Message = "Shift assignment created successfully",
            Data = assignment
        });
    }
    [HttpGet("{id:int}/requests")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<ActionResult<ApiResponse<ShiftAssignmentDetailsDto>>>
        GetRequestById(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _employeeRequestService
        .GetById(
            id,
            cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:int}/assets")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> GetAssets(
    int id,
    CancellationToken cancellationToken)
    {
        var result = await _assetService
            .GetEmployeeAssetsAsync(
                id,
                cancellationToken);

        return Ok(result);
    }
    [HttpGet("{employeeId:int}/performance-reviews")]
    [Authorize(Roles = "Admin,HRManager")]
    public async Task<IActionResult> GetEmployeePerformanceReviews(
        int employeeId,
        CancellationToken cancellationToken)
    {
        var history = await _reviewService
            .GetEmployeePerformanceReviewsAsync(
                employeeId,
                cancellationToken);

        var response = new ApiResponse<
            List<PerformanceReviewHistoryDto>>
        {
            Success = true,
            Message = "Employee Performance Review History",
            Data = history
        };

        return Ok(response);
    }
    [Authorize(Roles = "Employee")]
    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile(
        CancellationToken cancellationToken)
    {
        var result = await _employeeService.GetMyProfileAsync(cancellationToken);

        if(result == null)
            return NotFound();

        return Ok(new ApiResponse<EmployeeDetailsDto?>
        {
            Success = true,
            Message = $"Employee ID: {result?.Id} has found",
            Data = result
        });
    }
}
