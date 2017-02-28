using System;
using Bot.Tools.Interfaces;

namespace Bot.Tools {
  public class TimeService : ITimeService {
    private readonly ISettings _settings;

    public TimeService(ISettings settings) {
      _settings = settings;
    }

    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime DestinyNow => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(UtcNow, _settings.DestinyTimeZone);
    public DateTime DebuggerNow => TimeZoneInfo.ConvertTimeBySystemTimeZoneId(UtcNow, _settings.DebuggerTimeZone);

    public static DateTime UnixEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  }
}
