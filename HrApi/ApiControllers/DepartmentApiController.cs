using AutoMapper;
using HrApi.DTOs.Departments;
using HrApi.Interfaces;
using HrApi.Mapping;
using HrApi.Models;
using HrApi.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.Controllers;

[ApiController]
[Route("api/department")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DepartmentApiController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    public DepartmentApiController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }
    [HttpGet]
    public ActionResult<List<DepartmentListDto>> GetAll()
    {
        var result = _departmentService.GetAll();
        return Ok(result);
    }
    [HttpGet("{id}")]
    public ActionResult<ApiResponse<DepartmentDetailsDto>> GetById(int id)
    {
        var department = _departmentService.GetById(id);

        if (department == null)
            return NotFound(new ApiErrorResponse
            {
                Message = "دپارتمان مورد نظر پیدا نشد"
            });

        return Ok(new ApiResponse<DepartmentDetailsDto>
        {
            Success = true,
            Message = "اطلاعات دپارتمان دریافت شد",
            Data = department
        });
    }
    [HttpPost]
    public ActionResult<DepartmentDetailsDto> Create([FromBody]CreateDepartmentDto model)
    {
        try
        {
            var department = _departmentService.Create(model);
            return CreatedAtAction(
            nameof(GetById),
            new { id = department.Id },
            department);
        }
        catch(InvalidOperationException ex)
        {
            return Conflict(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    [HttpPut("{id}")]
    public IActionResult Update(int id, UpdateDepartmentDto model)
    {
        try
        {
            _departmentService.Update(id, model);
            return NoContent(); 
        }
        catch(InvalidOperationException ex)
        {
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            _departmentService.Delete(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new ApiErrorResponse
            {
                Message = ex.Message
            });
        }
    }
}
