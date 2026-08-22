using HrApi.DTOs.Candidates;
using HrApi.DTOs.Interviews;

namespace HrApi.Interfaces;

public interface IRecruitmentService
{
    Task<int> CreateCandidateAsync(
        CreateCandidateRequest request,
        CancellationToken cancellationToken);

    Task ScheduleInterviewAsync(
        int candidateId,
        ScheduleInterviewRequest request,
        CancellationToken cancellationToken);

    Task CompleteInterviewAsync(
        int candidateId,
        CompleteInterviewRequest request,
        CancellationToken cancellationToken);

    Task AcceptCandidateAsync(
        int candidateId,
        CancellationToken cancellationToken);

    Task RejectCandidateAsync(
        int candidateId,
        RejectCandidateRequest request,
        CancellationToken cancellationToken);

    Task ReopenCandidateAsync(
        int candidateId,
        CancellationToken cancellationToken);

    Task<int> HireCandidateAsync(
        int candidateId,
        HireCandidateRequest request,
        CancellationToken cancellationToken);
}
