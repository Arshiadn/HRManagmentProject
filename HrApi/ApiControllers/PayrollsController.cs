using HrApi.Data.Services;
using HrApi.DTOs.Payroll;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HrApi.ApiControllers;

[Route("api/payroll")]
[ApiController]
public class PayrollsController : ControllerBase
{
    private readonly PayrollService _payrollService;
    public PayrollsController(PayrollService payrollService) 
        => _payrollService = payrollService;

    [HttpGet("preview")]
    public async Task<ActionResult<PayrollPreviewDto>> GetPreview(
        int employeeId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var result = await _payrollService.GetPreviewAsync(
            employeeId, from, to, cancellationToken);

        return Ok(result);
    }
    [HttpGet("department-report")]
    public async Task<ActionResult<List<DepartmentPayrollReportDto>>>
    GetDepartmentPayrollReport(
        [FromQuery] DateOnly from,
        [FromQuery] DateOnly to,
        CancellationToken cancellationToken)
    {
        var result = await _payrollService
            .GetDepartmentPayrollReportAsync(
                from, to, cancellationToken);

        return Ok(result);
    }
}
