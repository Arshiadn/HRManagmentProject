namespace HrApi.ValueObjects;

public sealed record TimeRange
{
    public TimeOnly Start { get; } 
    public TimeOnly End { get; }

    private TimeRange(TimeOnly start, TimeOnly end)
    {
        if (start == end)
            throw new ArgumentException(
                "Start and end cannot be equal.");

        Start = start;
        End = end;
    }
    public static TimeRange Create(
        TimeOnly start, TimeOnly end) => new(start, end);

    public bool CrossesMidnight => End < Start;

    public WorkDuration Duration
    {
        get
        {
            var start = Start.Hour * 60 + Start.Minute;
            var end = End.Hour * 60 + End.Minute;

            if (CrossesMidnight)
                end += 24 * 60;

            return WorkDuration.FromMinutes(end - start);
        }
    }
}
