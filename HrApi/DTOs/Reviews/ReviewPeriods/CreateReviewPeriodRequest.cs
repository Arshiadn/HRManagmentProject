namespace HrApi.DTOs.Reviews.ReviewPeriods;

public sealed class CreateReviewPeriodRequest
{
    public string Title { get; init; } = null!;
    public DateOnly StartsOn { get; init; }
    public DateOnly EndsOn { get; init; }
    public Guid RubricId { get; init; }
}
