using HrApi.ValueObjects;

namespace HrApi.Interfaces;

public interface ILeaveBalanceService
{
    Task<LeaveBalance> GetBalanceAsync(
        int employeeId,
        CancellationToken cancellationToken);
}