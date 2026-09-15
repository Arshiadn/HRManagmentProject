using HrApi.Models.Performance.Rubric;
using HrApi.Models.Performance.Review;
using HrApi.DTOs.Reviews.ReviewRubrics;
using HrApi.DTOs.Reviews.ReviewPeriods;
using HrApi.DTOs.Reviews.PerformanceReview;

namespace HrApi.Interfaces;

public interface IReviewService
{
    Task<ReviewRubric> CreateRubricVesionAsync(
        CreateReviewRubricRequest request,
        CancellationToken cancellationToken);

    Task<ReviewPeriodResponseDto> CreateReviewPeriodAsync(
        CreateReviewPeriodRequest request,
        CancellationToken cancellationToken);

    Task<PerformanceReviewResponseDto> CreatePerformanceReviewAsync(
        CreatePerformanceReviewDto request,
        CancellationToken cancellationToken);

    Task<PerformanceReviewResponseDto> UpdatePerformanceReviewAsync(
        Guid id,
        UpdatePerformanceReviewDto request,
        CancellationToken cancellationToken);

    Task<PerformanceReviewResponseDto> SubmitPerformanceReviewAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<PerformanceReviewResponseDto> AcknowledgePerformanceReviewAsync(
        Guid id,
        string? employeeComment,
        CancellationToken cancellationToken);

    Task<PerformanceReviewResponseDto> ClosePerformanceReviewAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<List<PerformanceReviewHistoryDto>> GetEmployeePerformanceReviewsAsync(
        int employeeId,
        CancellationToken cancellationToken);
}
