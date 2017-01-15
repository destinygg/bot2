using System;
using Bot.Tools.Contracts;

namespace Bot.Tools {
  public class TimeService : ITimeService {
    public DateTime UtcNow => DateTime.UtcNow;
    public static DateTime UnixEpoch => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
  }
}
