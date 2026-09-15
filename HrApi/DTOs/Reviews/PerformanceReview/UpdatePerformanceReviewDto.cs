using HrApi.DTOs.Reviews.ReviewScores;

namespace HrApi.DTOs.Reviews.PerformanceReview;

public sealed class UpdatePerformanceReviewDto
{
    public List<UpdateReviewScoreDto> Scores { get; init; } = [];
}
