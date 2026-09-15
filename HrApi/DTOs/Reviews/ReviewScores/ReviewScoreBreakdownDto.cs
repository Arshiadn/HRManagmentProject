namespace HrApi.DTOs.Reviews.ReviewScores;

public sealed class ReviewScoreBreakdownDto
{
    public string CriterionCode { get; init; } = null!;
    public decimal Weight { get; init; }
    public int? Score { get; init; }
    public decimal WeightedScore { get; init; }
}
