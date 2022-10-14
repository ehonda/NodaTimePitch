using JetBrains.Annotations;
using NodaTime;

namespace NodaTimePitch;

[PublicAPI]
public static class LocalDateTimeExtensions
{
    public static LocalDate PreviousNth(this LocalDate now, IsoDayOfWeek dayOfWeek, int n)
        => n switch
        {
            > 1 => now.PreviousNth(dayOfWeek, 1).PreviousNth(dayOfWeek, n - 1),
            1 => now.Previous(dayOfWeek),
            0 => now,
            _ => throw new InvalidOperationException("Can't compute previous nth day of week for negative n")
        };
}
