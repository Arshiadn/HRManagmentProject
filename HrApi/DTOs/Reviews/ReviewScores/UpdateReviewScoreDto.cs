namespace HrApi.DTOs.Reviews.ReviewScores;

public sealed class UpdateReviewScoreDto
{
    public string CriterionCode { get; init; } = null!;
    public int Score { get; init; }
    public string? Comment { get; init; }
}
