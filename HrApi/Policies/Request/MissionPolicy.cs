using HrApi.Enums.Request;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;

namespace HrApi.Policies;

public sealed class MissionPolicy : IRequestPolicy
{
    public RequestType Type => RequestType.Mission;

    public Task ValidateAsync(
        EmployeeRequest request,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Destination))
            throw new BusinessRuleException(
                "Mission destination is required.");

        if (string.IsNullOrWhiteSpace(request.Purpose))
            throw new BusinessRuleException(
                "Mission purpose is required.");

        return Task.CompletedTask;
    }
}
