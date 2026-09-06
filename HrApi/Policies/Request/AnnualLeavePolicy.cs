using HrApi.Enums.Request;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;

namespace HrApi.Policies;

public sealed class AnnualLeavePolicy : IRequestPolicy
{
    private readonly ILeaveBalanceService _leaveBalanceService;
    public RequestType Type => RequestType.AnnualLeave;

    public AnnualLeavePolicy(ILeaveBalanceService leaveBalanceService)
        => _leaveBalanceService = leaveBalanceService;

    public async Task ValidateAsync(
        EmployeeRequest request,
        CancellationToken cancellationToken)
    {
        var balance = await _leaveBalanceService
            .GetBalanceAsync(request.EmployeeId, cancellationToken);

        if(request.TotalDays > balance.Days)
        {
            throw new BusinessRuleException(
                "Insufficient leave balance.");
        }
    }
}
