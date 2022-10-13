using FluentAssertions;
using NodaTime;
using NodaTime.Text;

namespace NodaTimePitch.Tests;

public class ZonedDateTimeExtensionsTests
{
    // TODO: Further Test cases
    //          - now is not at 00:00:00
    //          - Test GetNthIsoDayOfWeekBefore separately
    //              - Arrive in an invalid interval when converting to zoned
    //          - parameterize -> test for all days of a week
    
    // TODO: Description, name
    // TODO: Parameterize
    [Test]
    [Description("Week before should be returned from Wednesday")]
    public void Wednesday()
    {
        // Calendar for August 2022
        // Mon Tue Wed Thu Fri Sat Sun
        //                           2
        //   3   4   5   6   7   8   9
        //  10  11  12  13  14  15  16
        
        // TODO: Better variable names
        // TODO: Better date creation?
        var localDateTime = LocalDateTimePattern.GeneralIso.Parse("2022-10-12T00:00:00").Value;
        var now = new ZonedDateTime(localDateTime, DateTimeZone.Utc, Offset.Zero);

        var reportPeriod = now.GetReportPeriod();

        var expectedStart = new ZonedDateTime(
            LocalDateTimePattern.GeneralIso.Parse("2022-10-02T00:00:00").Value,
            DateTimeZone.Utc,
            Offset.Zero);
        var expectedEnd = new ZonedDateTime(
            LocalDateTimePattern.GeneralIso.Parse("2022-10-08T23:59:59").Value,
            DateTimeZone.Utc,
            Offset.Zero);
        
        reportPeriod.Start.Should().Be(expectedStart);
        reportPeriod.End.Should().Be(expectedEnd);
    }
    
    [Test]
    [Description("Two weeks before should be returned from Tuesday")]
    public void Tuesday()
    {
        // Calendar for September / October 2022
        // Mon Tue Wed Thu Fri Sat Sun
        //                          25
        //  26  27  28  29  30   1   2
        //   3   4   5   6   7   8   9
        //  10  11  12  13  14  15  16
        
        // TODO: Better variable names
        // TODO: Better date creation?
        var localDateTime = LocalDateTimePattern.GeneralIso.Parse("2022-10-11T00:00:00").Value;
        var now = new ZonedDateTime(localDateTime, DateTimeZone.Utc, Offset.Zero);

        var reportPeriod = now.GetReportPeriod();

        var expectedStart = new ZonedDateTime(
            LocalDateTimePattern.GeneralIso.Parse("2022-09-25T00:00:00").Value,
            DateTimeZone.Utc,
            Offset.Zero);
        var expectedEnd = new ZonedDateTime(
            LocalDateTimePattern.GeneralIso.Parse("2022-10-01T23:59:59").Value,
            DateTimeZone.Utc,
            Offset.Zero);
        
        reportPeriod.Start.Should().Be(expectedStart);
        reportPeriod.End.Should().Be(expectedEnd);
    }
}