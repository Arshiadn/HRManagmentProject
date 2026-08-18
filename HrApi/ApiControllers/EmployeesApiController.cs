using AutoMapper;
using HrApi.DTOs.Employees;
using HrApi.DTOs.Paging;
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
[AllowAnonymous]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class EmployeesApiController : ControllerBase
{
    private readonly IEmployeeService _employeeService;
    public EmployeesApiController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    [HttpGet]
    public ActionResult<List<EmployeeListDto>> GetAll()
    {
        var result = _employeeService.GetAll();
        return Ok(result);
    }
    [HttpGet("{id}")]
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
    public IActionResult Update(int id, UpdateEmployeeDto model)
    {
            _employeeService.Update(id, model);
            return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
            _employeeService.Delete(id);
            return NoContent();
    }
    [HttpGet("search")]
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
    [HttpPost("{id:int}/photo")]
    [Consumes("multipart/form-data")]
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
    public async Task<IActionResult> DeletePhoto(int id)
    {
            await _employeeService.DeletePhotoAsync(id);
            return NoContent();
    }
    [HttpPut("{id}/personnel-code")]
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
}
