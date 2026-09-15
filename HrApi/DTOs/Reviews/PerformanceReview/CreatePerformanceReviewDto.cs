namespace HrApi.DTOs.Reviews.PerformanceReview;

public sealed class CreatePerformanceReviewDto
{
    public Guid ReviewPeriodId { get; init; }
    public int EmployeeId { get; init; }
}
