using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class TimeService : ITimeService {
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime DestinyNow => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(UtcNow, Settings.DestinyTimeZone);
    public DateTime DebuggerNow => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(UtcNow, Settings.DebuggerTimeZone);

    public static DateTime UnixEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  }
}
