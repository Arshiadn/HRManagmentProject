using HrApi.Exceptions;
using HrApi.ValueObjects;

namespace HrApi.Calculators;

public static class WorkTimeCalculator
{
    public static DateOnly GetWorkDate(
        DateTimeOffset timestamp,
        TimeZoneInfo companyTimezone)
    {
        var local = TimeZoneInfo.ConvertTime(
            timestamp, companyTimezone);

        return DateOnly.FromDateTime(local.DateTime);
    }
    public static WorkDuration CalculateLateArrival(
        TimeOnly actualCheckIn,
        TimeRange workingHours,
        int graceMinutes)
    {
        var actual =
            actualCheckIn.Hour * 60 +
            actualCheckIn.Minute;

        var expected =
            workingHours.Start.Hour * 60 +
            workingHours.Start.Minute +
            graceMinutes;

        return WorkDuration.FromMinutes(
            Math.Max(0, actual - expected));
    }

    public static WorkDuration CalculateWorkedTime(
        DateTimeOffset checkIn,
        DateTimeOffset checkOut)
    {
        if(checkOut <= checkIn)
        {
            throw new BusinessRuleException("Check-out must be after check-in.");
        }

        return WorkDuration.FromMinutes(
            (int)Math.Round((checkOut - checkIn).TotalMinutes));
    }
    public static WorkDuration CalculateOvertime(
        WorkDuration worked,
        WorkDuration expected)
    {
        if (worked.Minutes <= expected.Minutes)
            return WorkDuration.Zero();

        return worked - expected;
    }
    public static WorkDuration CalculateEarlyLeave(
        DateTimeOffset checkOut,
        TimeRange workingHours,
        TimeZoneInfo companyTimeZone)
    {
        var localCheckOut = TimeZoneInfo.ConvertTime(
            checkOut,
            companyTimeZone);

        var workDate = DateOnly.FromDateTime(
            localCheckOut.DateTime);

        var shiftEnd = workDate
            .ToDateTime(workingHours.End);

        if (workingHours.CrossesMidnight)
        {
            shiftEnd = shiftEnd.AddDays(1);
        }

        var shiftEndAt = new DateTimeOffset(
            shiftEnd,
            localCheckOut.Offset);

        if (localCheckOut >= shiftEndAt)
        {
            return WorkDuration.Zero();
        }

        return WorkDuration.FromMinutes(
            (int)Math.Round(
                (shiftEndAt - localCheckOut).TotalMinutes));
    }
}
