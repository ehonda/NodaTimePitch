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
    private static class October2022Dates
    {
        public static IEnumerable<LocalDate> Mondays => LocalDatesForSameWeekday(3);
        public static IEnumerable<LocalDate> Tuesdays => LocalDatesForSameWeekday(4);
        public static IEnumerable<LocalDate> Wednesdays => LocalDatesForSameWeekday(5);
        public static IEnumerable<LocalDate> Thursdays => LocalDatesForSameWeekday(6);
        public static IEnumerable<LocalDate> Fridays => LocalDatesForSameWeekday(7);
        public static IEnumerable<LocalDate> Saturdays => LocalDatesForSameWeekday(8);
        public static IEnumerable<LocalDate> Sundays => LocalDatesForSameWeekday(9);

        private static IEnumerable<LocalDate> LocalDatesForSameWeekday(int startDay)
            => DaysForSameWeekday(startDay).Select(day => day.October(2022));

        private static IEnumerable<int> DaysForSameWeekday(int startDay)
            => Enumerable.Range(0, 4).Select(offset => startDay + offset * 7);
    }

    [Test]
    [Description("Previous nth saturday is calculated correctly for the last sunday in october 2022.")]
    public void PreviousNth_saturday_for_last_sunday([Values(0, 1, 2, 3, 4)] int n)
    {
        var lastSunday = October2022Dates.Sundays.Last();
        var previousSaturday = lastSunday.PreviousNth(IsoDayOfWeek.Saturday, n);
        var expectedPreviousSaturday = October2022Dates.Saturdays.Reverse().Prepend(lastSunday).ElementAt(n);

        previousSaturday.Should().Be(expectedPreviousSaturday);
    }
}