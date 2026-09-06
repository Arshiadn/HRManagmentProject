using HrApi.Enums.Request;
using HrApi.Exceptions;
using HrApi.Interfaces;
using HrApi.Models;

namespace HrApi.Policies;

public sealed class SickLeavePolicy : IRequestPolicy
{
    public RequestType Type => RequestType.SickLeave;

    public Task ValidateAsync(
        EmployeeRequest request,
        CancellationToken cancellationToken)
    {
        if (request.TotalDays > 2 &&
        string.IsNullOrWhiteSpace(request.AttachmentPath))
            throw new BusinessRuleException(
                "Medical document is required.");

        return Task.CompletedTask;
    }
}
