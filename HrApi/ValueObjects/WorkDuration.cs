using System.Net.NetworkInformation;

namespace HrApi.ValueObjects;

public readonly record struct WorkDuration
{
    public int Minutes { get; }

    private WorkDuration(int minutes)
    {
        if (minutes < 0)
            throw new ArgumentOutOfRangeException(nameof(minutes));

        Minutes = minutes;
    }
    public static WorkDuration FromMinutes(int minutes)
        => new(minutes);
    public static WorkDuration Zero() => new(0);

    public static WorkDuration operator +(
        WorkDuration left, WorkDuration right) => new(left.Minutes + right.Minutes);
    public static WorkDuration operator -(
        WorkDuration left, WorkDuration right) => new(Math.Max(0, left.Minutes - right.Minutes));
}
