using HrApi.Calculators;
using HrApi.DTOs.Contracts.ReadModel;
using HrApi.DTOs.Employees.ReadModel;
using HrApi.DTOs.Payroll;
using HrApi.Exceptions;
using HrApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class PayrollService
{
    private readonly HrDbContext _context;
    public PayrollService(HrDbContext context) => _context = context;

    public async Task<PayrollPreviewDto> GetPreviewAsync(
        int employeeId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var employee = await GetEmployeeAsync(
            employeeId, cancellationToken);

        var contract = await GetActiveContractAsync(
            employeeId, cancellationToken);

        var workedMinutes = await GetWorkedMinutesAsync(
            employeeId, from, to, cancellationToken);

        var overtimeMinutes = await GetOvertimeMinutesAsync(
            employeeId, from, to, cancellationToken);

        var deductionMinutes = await GetDeductionMinutesAsync(
            employeeId, from, to, cancellationToken);

        var context = new PayrollCalculationContext
        {
            BaseSalary = contract.BaseSalary,
            OvertimeMinutes = overtimeMinutes,
            DeductionMinutes = deductionMinutes
        };

        PayrollCalculationPipeline.ApplyBaseSalary(context);
        PayrollCalculationPipeline.ApplyOvertime(context);
        PayrollCalculationPipeline.ApplyDeduction(context);

        return MapToPreview(
            employee, workedMinutes, context);
    }
    private async Task<EmployeeIdReadModel> GetEmployeeAsync(
        int employeeId, CancellationToken cancellationToken)
    {
        return await _context.Employees
            .AsNoTracking()
            .Where(x => x.Id == employeeId)
            .Select(x => new EmployeeIdReadModel
            {
                Id = x.Id,
                FullName = x.FullName
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Employee not found.");
    }
    private async Task<ContractBaseSalaryReadModel> GetActiveContractAsync(
        int employeeId,
        CancellationToken cancellationToken)
    {
        return await _context.EmployeeContracts
            .Where(x => x.EmployeeId == employeeId && x.IsActive)
            .OrderByDescending(x => x.StartDate)
            .Select(x => new ContractBaseSalaryReadModel
            {
                BaseSalary = x.BaseSalary
            })
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Active contract not found.");
    }
    private async Task<int> GetWorkedMinutesAsync(
        int employeeId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var workedMinutes = await _context.AttendanceRecords
            .Where(x =>
                x.EmployeeId == employeeId &&
                x.WorkDate >= from &&
                x.WorkDate <= to)
            .SumAsync(
                x => x.WorkedMinutes,
                cancellationToken);

        return workedMinutes;
    }
    private async Task<int> GetOvertimeMinutesAsync(
        int employeeId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var overtimeMinutes = await _context.AttendanceRecords
            .Where(x =>
                x.EmployeeId == employeeId &&
                x.WorkDate >= from &&
                x.WorkDate <= to)
            .SumAsync(
                x => x.OvertimeMinutes,
                cancellationToken);

        return overtimeMinutes;
    }
    private async Task<int> GetDeductionMinutesAsync(
        int employeeId,
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var deductionMinutes = await _context.AttendanceRecords
            .Where(x =>
                x.EmployeeId == employeeId &&
                x.WorkDate >= from &&
                x.WorkDate <= to)
            .SumAsync(
                x => x.LateMinutes + x.EarlyLeaveMinutes,
                cancellationToken);

        return deductionMinutes;
    }
    private PayrollPreviewDto MapToPreview(
        EmployeeIdReadModel employee,
        int workedMinutes,
        PayrollCalculationContext context)
    {
        return new PayrollPreviewDto
        {
            EmployeeId = employee.Id,
            EmployeeName = employee.FullName.Trim(),
            BaseSalary = context.BaseSalary,
            WorkedMinutes = workedMinutes,
            OvertimeMinutes = context.OvertimeMinutes,
            DeductionMinutes = context.DeductionMinutes,
            OvertimeAmount = Math.Round(context.OvertimeAmount),
            DeductionAmount = Math.Round(context.DeductionAmount),
            PreviewAmount = Math.Round(context.Total)
        };
    }
    public async Task<List<DepartmentPayrollReportDto>>
    GetDepartmentPayrollReportAsync(
        DateOnly from,
        DateOnly to,
        CancellationToken cancellationToken)
    {
        var employees = await _context.Employees
            .AsNoTracking()
            .Where(x => x.IsActive)
            .Select(x => new
            {
                EmployeeId = x.Id,
                DepartmentId = x.DepartmentId,
                DepartmentName = x.Department.Name,

                BaseSalary = x.Contracts
                    .Where(c => c.IsActive)
                    .OrderByDescending(c => c.StartDate)
                    .Select(c => (decimal?)c.BaseSalary)
                    .FirstOrDefault() ?? 0,

                WorkedMinutes = x.AttendanceRecords
                    .Where(a =>
                        a.WorkDate >= from &&
                        a.WorkDate <= to)
                    .Sum(a => (int?)a.WorkedMinutes) ?? 0,

                OvertimeMinutes = x.AttendanceRecords
                    .Where(a =>
                        a.WorkDate >= from &&
                        a.WorkDate <= to)
                    .Sum(a => (int?)a.OvertimeMinutes) ?? 0,

                DeductionMinutes = x.AttendanceRecords
                    .Where(a =>
                        a.WorkDate >= from &&
                        a.WorkDate <= to)
                    .Sum(a =>
                        (int?)(a.LateMinutes +
                               a.EarlyLeaveMinutes)) ?? 0
            })
            .ToListAsync(cancellationToken);

        var payrolls = employees
            .Select(x =>
            {
                var context = new PayrollCalculationContext
                {
                    BaseSalary = x.BaseSalary,
                    OvertimeMinutes = x.OvertimeMinutes,
                    DeductionMinutes = x.DeductionMinutes
                };

                PayrollCalculationPipeline.ApplyBaseSalary(context);
                PayrollCalculationPipeline.ApplyOvertime(context);
                PayrollCalculationPipeline.ApplyDeduction(context);

                return new
                {
                    x.DepartmentId,
                    x.DepartmentName,
                    BaseSalary = context.BaseSalary,
                    OvertimeAmount = context.OvertimeAmount,
                    DeductionAmount = context.DeductionAmount,
                    PreviewAmount = context.Total
                };
            })
            .ToList();

        var report = payrolls
            .GroupBy(x => new
            {
                x.DepartmentId,
                x.DepartmentName
            })
            .Select(g => new DepartmentPayrollReportDto
            {
                DepartmentId = g.Key.DepartmentId,
                DepartmentName = g.Key.DepartmentName,
                EmployeeCount = g.Count(),

                TotalBaseSalary = g.Sum(x => x.BaseSalary),

                TotalOvertime = g.Sum(x => x.OvertimeAmount),

                TotalDeductions = g.Sum(x => x.DeductionAmount),

                TotalPreview = g.Sum(x => x.PreviewAmount)
            })
            .ToList();

        return report;
    }
}
