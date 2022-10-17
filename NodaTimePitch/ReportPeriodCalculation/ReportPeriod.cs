using NodaTime;

namespace NodaTimePitch.ReportPeriodCalculation;

public record ReportPeriod(ZonedDateTime Start, ZonedDateTime End);
