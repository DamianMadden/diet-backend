namespace draft_ml.Extensions;

public static class DateTimeExtensions
{
    public static int CalendarYearDifference(this DateTime a, DateTime b)
    {
        ref DateTime start = ref (a > b ? ref b : ref a);
        ref DateTime end = ref (a > b ? ref a : ref b);

        var years = end.Year - start.Year - 1;

        if (end.Month > start.Month || ((end.Month == start.Month) && (end.Day >= start.Day)))
            years++;
        return years;
    }
}
