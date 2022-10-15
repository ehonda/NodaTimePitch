using FluentAssertions;
using NodaTime;
using NodaTime.Testing.Extensions;

namespace NodaTimePitch.Tests;

public class LocalDateTimeExtensionsTests
{
    // TODO: Use fake calendar for testing?
    // Calendar for October 2022
    // Mon Tue Wed Thu Fri Sat Sun
    //   3   4   5   6   7   8   9
    //  10  11  12  13  14  15  16
    //  17  18  19  20  21  22  23
    //  24  25  26  27  28  29  30
    private static class October2022
    {
        private static IEnumerable<LocalDate> Mondays => LocalDatesForSameWeekday(3);
        private static IEnumerable<LocalDate> Tuesdays => LocalDatesForSameWeekday(4);
        private static IEnumerable<LocalDate> Wednesdays => LocalDatesForSameWeekday(5);
        private static IEnumerable<LocalDate> Thursdays => LocalDatesForSameWeekday(6);
        private static IEnumerable<LocalDate> Fridays => LocalDatesForSameWeekday(7);
        private static IEnumerable<LocalDate> Saturdays => LocalDatesForSameWeekday(8);
        private static IEnumerable<LocalDate> Sundays => LocalDatesForSameWeekday(9);

        public static IEnumerable<LocalDate> LocalDatesWithDayOfWeek(IsoDayOfWeek dayOfWeek)
            => dayOfWeek switch
            {
                IsoDayOfWeek.Monday => Mondays,
                IsoDayOfWeek.Tuesday => Tuesdays,
                IsoDayOfWeek.Wednesday => Wednesdays,
                IsoDayOfWeek.Thursday => Thursdays,
                IsoDayOfWeek.Friday => Fridays,
                IsoDayOfWeek.Saturday => Saturdays,
                IsoDayOfWeek.Sunday => Sundays,
                _ => throw new ArgumentOutOfRangeException(nameof(dayOfWeek), dayOfWeek, "Invalid day of week provided")
            };

        private static IEnumerable<LocalDate> LocalDatesForSameWeekday(int startDay)
            => DaysForSameWeekday(startDay).Select(day => day.October(2022));

        private static IEnumerable<int> DaysForSameWeekday(int startDay)
            => Enumerable.Range(0, 4).Select(offset => startDay + offset * 7);
    }
    
    private static class ValueSources
    {
        public static IEnumerable<IsoDayOfWeek> IsoDaysOfWeek => Enum.GetValues<IsoDayOfWeek>().Except(new[]
        {
            IsoDayOfWeek.None
        });
    }

    // TODO: Test case for month transition
    [Test]
    [Description("Previous nth is calculated correctly for a subset of combinations of source and target days " +
                 "in october 2022.")]
    public void PreviousNth_for_combinations_in_october_2022(
        [ValueSource(typeof(ValueSources), nameof(ValueSources.IsoDaysOfWeek))] IsoDayOfWeek sourceDay,
        [ValueSource(typeof(ValueSources), nameof(ValueSources.IsoDaysOfWeek))] IsoDayOfWeek targetDay,
        [Values(0, 1, 2, 3)] int n)
    {
        var sourceDate = October2022.LocalDatesWithDayOfWeek(sourceDay).Last();
        var previousNth = sourceDate.PreviousNth(targetDay, n);
        
        // Expected previous nth is calculated as follows:
        // - For a source date (e.g. Fri 28) take all dates for the given target day (e.g. Sat) that are before
        //   the source date. In this example that would be calendar days [8, 15, 22].
        // - Reverse those candidates, as we are "moving backward in time" from the source date
        // - Prepend the source date, which is returned in the n = 0 case
        // - Take the nth element to get the expected previous nth
        var expectedPreviousNth = October2022
            .LocalDatesWithDayOfWeek(targetDay)
            .Reverse()
            .Where(date => date < sourceDate)
            .Prepend(sourceDate)
            .ElementAt(n);

        previousNth.Should().Be(expectedPreviousNth);
    }
}