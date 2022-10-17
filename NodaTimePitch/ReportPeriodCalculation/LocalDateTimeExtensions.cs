using JetBrains.Annotations;
using NodaTime;

namespace NodaTimePitch.ReportPeriodCalculation;

[PublicAPI]
public static class LocalDateTimeExtensions
{
    // TODO: Benchmark recursive version from 52cff2b against this non recursive version with PlusWeeks
    // TODO: Can we make this Pure by not throwing exceptions? PlusWeeks is marked as Pure but can throw which seems odd
    public static LocalDate PreviousNth(this LocalDate sourceDate, IsoDayOfWeek targetDayOfWeek, int n)
        => n switch
        {
            >= 1 => sourceDate.Previous(targetDayOfWeek).PlusWeeks(-n + 1),
            0 => sourceDate,
            _ => throw new InvalidOperationException("Can't compute previous nth day of week for negative n")
        };
}
