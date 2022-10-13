using JetBrains.Annotations;
using NodaTime;

namespace NodaTimePitch;

[PublicAPI]
public static class ZonedDateTimeExtensions
{
    [Pure]
    public static (ZonedDateTime Start, ZonedDateTime End) GetReportPeriod(this ZonedDateTime now)
        => now.DayOfWeek switch
        {
            < IsoDayOfWeek.Wednesday => now.GetNthReportPeriodBefore(2),
            _ => now.GetNthReportPeriodBefore(1)
        };

    [Pure]
    private static (ZonedDateTime Start, ZonedDateTime End) GetNthReportPeriodBefore(this ZonedDateTime now, uint n)
        => (
            now.GetNthIsoDayOfWeekBefore(IsoDayOfWeek.Sunday, n + 1),
            now.GetNthIsoDayOfWeekBefore(IsoDayOfWeek.Sunday, n) - Duration.FromSeconds(1));

    // TODO: Version for LocalDateTime, only use that one
    [Pure]
    public static ZonedDateTime GetNthIsoDayOfWeekBefore(this ZonedDateTime now, IsoDayOfWeek dayOfWeek, uint n) =>
        n switch
        {
            > 1 => now.GetNthIsoDayOfWeekBefore(dayOfWeek, 1).GetNthIsoDayOfWeekBefore(dayOfWeek, n - 1),
            // TODO: Are there cases where this fails because the offset is wrong?
            1 => new(now.LocalDateTime.Previous(dayOfWeek), now.Zone, now.Offset),
            // TODO: Throw on 0 or return now, which one makes more sense?
            0 => now
        };
}
