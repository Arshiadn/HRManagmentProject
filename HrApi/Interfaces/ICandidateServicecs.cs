using HrApi.DTOs.Candidates;
using HrApi.DTOs.Paging;
using HrApi.Responses;

namespace HrApi.Interfaces;

public interface ICandidateServicecs
{
    Task<int> CreateCandidateAsync(
        CreateCandidateRequest request,
        CancellationToken cancellationToken);

    Task<PagedResultDto<CandidateDetailsDto>> GetListAsync(
        CandidateListRequest request,
        CancellationToken cancellationToken);

    Task<ApiResponse<CandidateDetailsDto>> GetByIdAsync(
        int id,
        CancellationToken cancellationToken);
}
