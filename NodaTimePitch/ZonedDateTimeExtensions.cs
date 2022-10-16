using JetBrains.Annotations;
using NodaTime;

namespace NodaTimePitch;

[PublicAPI]
// TODO: Can we make the functions pure again (e.g. by returning Maybe)?
// TODO: Use record for reportPeriod
public static class ZonedDateTimeExtensions
{
    public static (ZonedDateTime Start, ZonedDateTime End) GetReportPeriod(this ZonedDateTime now)
        => now.DayOfWeek switch
        {
            < IsoDayOfWeek.Wednesday => now.GetNthReportPeriodBefore(2),
            _ => now.GetNthReportPeriodBefore(1)
        };

    private static (ZonedDateTime Start, ZonedDateTime End) GetNthReportPeriodBefore(
        this ZonedDateTime sourceDateTime, int n)
        => (
            sourceDateTime.PreviousNth(IsoDayOfWeek.Sunday, n + 1, LocalTime.Midnight),
            sourceDateTime.PreviousNth(IsoDayOfWeek.Sunday, n, LocalTime.Midnight) - Duration.FromSeconds(1));

    // TODO: Provide variant with ZoneLocalMappingResolver -> Do we really need that?
    private static ZonedDateTime PreviousNth(this ZonedDateTime sourceDateTime, IsoDayOfWeek dayOfWeek, int n,
        LocalTime targetTimeOfDay)
    {
        var targetDate = sourceDateTime.LocalDateTime.Date.PreviousNth(dayOfWeek, n);
        return targetDate.At(targetTimeOfDay).InZoneLeniently(sourceDateTime.Zone);
    }
}
