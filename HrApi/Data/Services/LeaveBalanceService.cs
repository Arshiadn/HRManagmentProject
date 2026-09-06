using HrApi.Interfaces;
using HrApi.ValueObjects;
using HrApi.Enums;
using HrApi.Enums.Request;
using Microsoft.EntityFrameworkCore;

namespace HrApi.Data.Services;

public sealed class LeaveBalanceService : ILeaveBalanceService
{
    private const int AnnualLeaveDays = 24;

    private readonly HrDbContext _context;

    public LeaveBalanceService(HrDbContext context) => _context = context;

    public async Task<LeaveBalance> GetBalanceAsync(
        int employeeId, 
        CancellationToken cancellationToken)
    {
        var usedDays = await _context.EmployeeRequests
            .Where(
                x => x.EmployeeId == employeeId &&
                x.Type == RequestType.AnnualLeave &&
                x.Status == RequestStatus.Approved)
            .SumAsync(x => (int?)x.TotalDays, cancellationToken)
            ?? 0;

        var remainingDays = Math.Max(0, AnnualLeaveDays - usedDays);

        return LeaveBalance.FromDays(remainingDays);
    }
}