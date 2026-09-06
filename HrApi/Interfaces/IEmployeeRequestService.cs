using HrApi.DTOs.EmployeeRequests;
using HrApi.DTOs.Paging;
using HrApi.Responses;

namespace HrApi.Interfaces;

public interface IEmployeeRequestService
{
    Task<EmployeeRequestDetailsDto> CreateAsync(
        CreateEmployeeRequestDto dto,
        CancellationToken cancellationToken);

    Task SubmitAsync(
        long Id,
        CancellationToken cancellationToken);

    Task ApproveAsync(
    long id,
    CancellationToken cancellationToken);

    Task RejectAsync(
        long id,
        CancellationToken cancellationToken);

    Task CancelAsync(
        long id,
        CancellationToken cancellationToken);

    Task<ApiResponse<EmployeeRequestDetailsDto>> GetById(
        int employeeId,
        CancellationToken cancellationToken);

    Task<PagedResultDto<EmployeeRequestDetailsDto>> GetListAsync(
        EmployeeRequestListRequest request,
        CancellationToken cancellationToken);
}
