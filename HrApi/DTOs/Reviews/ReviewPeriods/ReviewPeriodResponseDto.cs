using HrApi.DTOs.Reviews.ReviewRubrics;

namespace HrApi.DTOs.Reviews.ReviewPeriods;

public sealed class ReviewPeriodResponseDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public DateOnly StartsOn { get; init; }
    public DateOnly EndsOn { get; init; }
    public bool IsClosed { get; init; }

    public RubricSummaryDto? Rubric { get; init; }
}
