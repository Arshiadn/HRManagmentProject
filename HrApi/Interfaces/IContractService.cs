using HrApi.DTOs.Contracts;
using HrApi.DTOs.Paging;
using HrApi.Responses;

namespace HrApi.Interfaces;

public interface IContractService
{
    Task<int> CreateContractAsync(
        CreateContractRequest request,
        CancellationToken cancellationToken);

    Task <PagedResultDto<ContractListItemDto>> GetListAsync(
        ContractListRequest request,
        DateOnly today,
        CancellationToken cancellationToken);

    Task<ApiResponse<ContractDetailsDto?>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);

    Task SubmitSignatureAsync(
        int contractId,
        SubmitSignatureRequest request,
        CancellationToken cancellationToken);

    Task ActivateContractAsync(
        int contractId,
        ActivateContractRequest request,
        CancellationToken cancellationToken);

    Task CancelContractAsync(
        int contractId,
        CancelContractRequest request,
        CancellationToken cancellationToken);

    Task RenewContractAsync(
        int contractId,
        string reason,
        CancellationToken cancellationToken);

    Task CompleteContractAsync(
        int contractId,
        CompleteContractRequest request,
        CancellationToken cancellationToken);
}
