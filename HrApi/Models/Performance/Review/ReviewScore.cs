using HrApi.Exceptions;

namespace HrApi.Models.Performance.Review;

public sealed class ReviewScore
{
    public long Id { get; private set; }

    public Guid PerformanceReviewId { get; private set; }
    public PerformanceReview PerformanceReview { get; private set; } = null!;

    public string CriterionCode { get; private set; } = null!;
    public int Score { get; private set; }
    public string? Comment { get; private set; }

    private ReviewScore() 
    {
    }

    public ReviewScore(
        Guid performanceReviewId,
        string criterionCode,
        int score,
        string? comment)
    {
        EnsureScore(criterionCode, score);

        PerformanceReviewId = performanceReviewId;
        CriterionCode = criterionCode;
        Score = score;
        Comment = comment;
    }

    public void Update(
    int score,
    string? comment)
    {
        EnsureValidScore(score);

        Score = score;
        Comment = comment?.Trim();
    }
    public static void EnsureValidScore(int score)
    {
        if (score is < 1 or > 5)
            throw new BadRequestException("امتیاز باید بین ۱ و ۵ باشد.");
    }

    public static void EnsureScore(string criterionCode, int score)
    {
        if (string.IsNullOrWhiteSpace(criterionCode))
            throw new BadRequestException("معیار الزامی است.");

        if (score is < 1 or > 5)
            throw new BadRequestException("امتیاز باید بین ۱ و ۵ باشد.");
    }
}