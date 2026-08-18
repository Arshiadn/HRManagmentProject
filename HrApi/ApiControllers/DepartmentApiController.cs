using AutoMapper;
using HrApi.DTOs.Departments;
using HrApi.DTOs.Employees;
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
[Route("api/departments")]
[AllowAnonymous]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class DepartmentApiController : ControllerBase
{
    private readonly IDepartmentService _departmentService;
    private readonly IEmployeeService _employeeService;
    public DepartmentApiController(IDepartmentService departmentService, IEmployeeService employeeService)
    {
        _departmentService = departmentService;
        _employeeService = employeeService;
    }
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DepartmentListDto>>>
    GetAll(CancellationToken cancellationToken)
    {
        var result =
            await _departmentService
                .GetAllAsync(cancellationToken);

        return Ok(new ApiResponse<IReadOnlyList<DepartmentListDto>>
        {
            Success = true,
            Message = "Deleted departments retrieved successfully",
            Data = result
        });
    }
    [HttpGet("deleted")]
    public async Task<ActionResult<IReadOnlyList<DepartmentListDto>>> GetDeletedList(CancellationToken cancellationToken)
    {
        var result = await _departmentService.GetDeletedListAsync(cancellationToken);

        return Ok(result);
    }
    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartmentDetailsDto>>
    GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var result =
            await _departmentService
                .GetByIdAsync(id, cancellationToken);

        return Ok(result);
    }
    [HttpPost]
    public async Task<ActionResult> Create(
    CreateDepartmentDto request,
    CancellationToken cancellationToken)
    {
        var id =
            await _departmentService
                .CreateAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            new { id });
    }
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
       int id,
       UpdateDepartmentDto request,
       CancellationToken cancellationToken)
    {
        await _departmentService
            .UpdateAsync(id, request, cancellationToken);

        return NoContent();
    }
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
    int id,
    CancellationToken cancellationToken)
    {
        await _departmentService
            .SoftDeleteAsync(id, cancellationToken);

        return NoContent();
    }
    [HttpPost("{id:int}/restore")]
    public async Task<IActionResult> Restore(
    int id,
    CancellationToken cancellationToken)
    {
        await _departmentService
            .RestoreAsync(id, cancellationToken);

        return NoContent();
    }
    [HttpGet("{id:int}/employees")]
    public async Task<IActionResult> GetEmployees(int id, CancellationToken cancellationToken)
    {
        var employees = await _departmentService.GetEmployeesAsync(id, cancellationToken);
        return Ok(new ApiResponse<List<EmployeeListDto>>
        {
            Success = true,
            Message = "Employees retrieved successfully",
            Data = employees
        });
    }
}
