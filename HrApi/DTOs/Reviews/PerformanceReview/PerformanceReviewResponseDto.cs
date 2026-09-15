using HrApi.DTOs.Reviews.ReviewScores;
using HrApi.Enums.Review;

namespace HrApi.DTOs.Reviews.PerformanceReview;

public sealed class PerformanceReviewResponseDto
{
    public Guid Id { get; init; }
    public Guid ReviewPeriodId { get; init; }
    public Guid ReviewRubricId { get; init; }

    public int EmployeeId { get; init; }

    public ReviewStatus Status { get; init; }
    public decimal? OverallScore { get; init; }

    public List<ReviewScoreBreakdownDto> Breakdown { get; init; } = [];
}
