using System.Net.NetworkInformation;

namespace HrApi.ValueObjects;

public readonly record struct LeaveBalance
{
    public int Days { get; }

    private LeaveBalance(int days)
    {
        if(days < 0)
            throw new ArgumentOutOfRangeException(nameof(days));

        Days = days;
    }

    public static LeaveBalance FromDays(int days)
        => new(days);

    public static LeaveBalance Zero => new(0);
}
