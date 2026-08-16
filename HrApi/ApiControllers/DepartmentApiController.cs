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
    public async Task<ActionResult<IReadOnlyList<DepartmentListDto>>>
    GetAll(CancellationToken cancellationToken)
    {
        var result =
            await _departmentService
                .GetAllAsync(cancellationToken);

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
}
