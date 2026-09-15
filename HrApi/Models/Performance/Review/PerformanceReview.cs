using HrApi.Enums.Review;
using HrApi.Exceptions;
using HrApi.Models.Performance.Audit;
using HrApi.Models.Performance.Rubric;

namespace HrApi.Models.Performance.Review;

public sealed class PerformanceReview
{
    public Guid Id { get; private set; }

    public Guid ReviewPeriodId { get; private set; }
    public ReviewPeriod ReviewPeriod { get; private set; } = null!;

    public Guid ReviewRubricId { get; private set; }
    public ReviewRubric ReviewRubric { get; private set; } = null!;

    public int EmployeeId { get; private set; }

    // Add this later
    //public int ManagerId { get; private set; }
    //public Employee Manager { get; set; } = null!;
    public ReviewStatus Status { get; private set; }
    public string? EmployeeComment { get; private set; }
    //public string? ManagerComment { get; private set; }
    public DateTimeOffset? SubmittedAt { get; private set; }
    public DateTimeOffset? AcknowledgedAt { get; private set; }
    public string? RubricSnapshotJson { get; private set; }
    public decimal? OverallScore { get; private set; }
    public byte[] RowVersion { get; private set; } = Array.Empty<byte>(); 
    public List<ReviewScore> Scores { get; private set; } = [];

    private PerformanceReview() { }

    public PerformanceReview(
    Guid reviewPeriodId,
    Guid reviewRubricId,
    int employeeId)
    {
        if (reviewPeriodId == Guid.Empty)
        {
            throw new BusinessRuleException(
                "Review period is required.");
        }
        if (reviewRubricId == Guid.Empty)
        {
            throw new BusinessRuleException(
                "Review rubric is required.");
        }
        if (employeeId <= 0)
        {
            throw new BusinessRuleException(
                "Employee is required.");
        }
        Id = Guid.NewGuid();
        ReviewPeriodId = reviewPeriodId;
        ReviewRubricId = reviewRubricId;
        EmployeeId = employeeId;
        Status = ReviewStatus.Draft;
    }

    public void SetFinalResult(
        decimal overallScore,
        string snapshotJson)
    {
        if (Status != ReviewStatus.Draft)
            throw new BusinessRuleException(
                "Only draft reviews can be finalize.");

        if (string.IsNullOrWhiteSpace(snapshotJson))
            throw new BusinessRuleException(
            "Review snapshot is required.");

        OverallScore = overallScore;
        RubricSnapshotJson = snapshotJson;
    }
    public void UpdateOverallScore(decimal overallScore)
    {
        if (Status != ReviewStatus.Draft)
            throw new BusinessRuleException(
                "Only draft reviews can be updated.");

        OverallScore = overallScore;
    }
    public void Submit(DateTimeOffset now)
    {
        if (Status != ReviewStatus.Draft)
            throw new BusinessRuleException(
                "فقط Review پیش‌نویس قابل ارسال است.");

        if (Scores.Count == 0)
            throw new BusinessRuleException(
                "حداقل یک معیار باید امتیازدهی شود.");

        Status = ReviewStatus.Submitted;
        SubmittedAt = now;
    }
    public void Acknowledge(
    string? employeeComment,
    DateTimeOffset now)
    {
        if (Status != ReviewStatus.Submitted)
            throw new BusinessRuleException(
                "فقط Review ارسال‌شده قابل تأیید است.");

        EmployeeComment = employeeComment?.Trim();
        AcknowledgedAt = now;
        Status = ReviewStatus.Acknowledged;
    }
    public void Close()
    {
        if (Status != ReviewStatus.Acknowledged)
        {
            throw new BusinessRuleException(
                "Only acknowledged reviews can be closed.");
        }

        Status = ReviewStatus.Closed;
    }
}
