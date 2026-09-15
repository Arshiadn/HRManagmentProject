using HrApi.Enums.Review;
using HrApi.Models.Performance.Review;

namespace HrApi.Models.Performance.Audit;

public sealed class ReviewAuditEntry
{
    public long Id { get; private set; }

    public Guid PerformanceReviewId { get; private set; }
    public PerformanceReview PerformanceReview { get; private set; } = null!;

    public string Action { get; private set; } = null!;
    public ReviewStatus? FromStatus { get; private set; }
    public ReviewStatus? ToStatus { get; private set; }
    public string ActorType { get; private set; } = null!;
    public int ActorId { get; private set; }
    public string? Reason { get; private set; }
    public DateTimeOffset OccurredAt { get; private set; }
}
