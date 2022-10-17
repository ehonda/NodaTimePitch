using FluentAssertions;
using NodaTime;
using NodaTime.Text;

namespace NodaTimePitch.Tests;

public class ZonedDateTimeExtensionsTests
{
    // TODO: Further Test cases
    //          - parameterize -> test for all days of a week
    // TODO: Helper functions to write test cases more cleanly
    // TODO: Cleanup

    private static void ExpectReportPeriod(string startPattern, string endPattern, ReportPeriod actualReportPeriod)
    {
        var expectedStart = new ZonedDateTime(
            LocalDateTimePattern.GeneralIso.Parse(startPattern).Value,
            DateTimeZone.Utc,
            Offset.Zero);
        var expectedEnd = new ZonedDateTime(
            LocalDateTimePattern.GeneralIso.Parse(endPattern).Value,
            DateTimeZone.Utc,
            Offset.Zero);
        
        actualReportPeriod.Start.Should().Be(expectedStart);
        actualReportPeriod.End.Should().Be(expectedEnd);        
    }
    
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

        ExpectReportPeriod("2022-10-02T00:00:00", "2022-10-08T23:59:59", reportPeriod);
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
        
        ExpectReportPeriod("2022-09-25T00:00:00", "2022-10-01T23:59:59", reportPeriod);
    }
    
    [Test]
    [Description("Test case not from midnight")]
    public void Not_calculated_from_midnight()
    {
        // Calendar for September / October 2022
        // Mon Tue Wed Thu Fri Sat Sun
        //                          25
        //  26  27  28  29  30   1   2
        //   3   4   5   6   7   8   9
        //  10  11  12  13  14  15  16
        
        // TODO: Better variable names
        // TODO: Better date creation?
        var localDateTime = LocalDateTimePattern.GeneralIso.Parse("2022-10-12T11:00:00").Value;
        var now = new ZonedDateTime(localDateTime, DateTimeZone.Utc, Offset.Zero);

        var reportPeriod = now.GetReportPeriod();
        
        ExpectReportPeriod("2022-10-02T00:00:00", "2022-10-08T23:59:59", reportPeriod);
    }
}