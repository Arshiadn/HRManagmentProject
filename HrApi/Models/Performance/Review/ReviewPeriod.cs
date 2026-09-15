using HrApi.Exceptions;
using HrApi.Models.Performance.Rubric;

namespace HrApi.Models.Performance.Review;

public sealed class ReviewPeriod
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public DateOnly StartsOn { get; private set; }
    public DateOnly EndsOn { get; private set; }
    public bool IsClosed { get; private set; }

    public Guid? SelectedRubricId { get; private set; }
    public ReviewRubric? SelectedRubric { get; private set; }

    private ReviewPeriod() { }

    private ReviewPeriod(
        string title,
        DateOnly startsOn,
        DateOnly endsOn)
    {
        EnsureValidDates(startsOn, EndsOn);
        EnsureValidTitle(title);

        Id = Guid.NewGuid();
        Title = title.Trim();
        StartsOn = startsOn;
        EndsOn = endsOn;
        IsClosed = false;
    }
    public static ReviewPeriod Create(
        string title,
        DateOnly startsOn,
        DateOnly endsOn)
    {
        return new ReviewPeriod(
            title, startsOn, endsOn);
    }
    public static void EnsureValidDates(
        DateOnly startsOn, DateOnly endsOn)
    {
        if (endsOn < startsOn)
            throw new BusinessRuleException(
                "پایان دوره نمی‌تواند قبل از شروع آن باشد.");
    }
    public static void EnsureValidTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new BusinessRuleException(
                "Review period title is required.");
        }
    }
    public void Close()
    {
        if (IsClosed) 
            throw new BusinessRuleException(
                "دوره قبلاً بسته شده است.");

        IsClosed = true;
    }
    public void SelectRubric(Guid rubricId)
    {
        if (rubricId == Guid.Empty)
        {
            throw new BusinessRuleException(
                "Rubric is required.");
        }

        SelectedRubricId = rubricId;
    }
}
