using HrApi.Enums.Request;
using HrApi.Models;

namespace HrApi.Interfaces;

public interface IRequestPolicy
{
    RequestType Type { get; }

    Task ValidateAsync(
        EmployeeRequest request,
        CancellationToken cancellationToken);
}
