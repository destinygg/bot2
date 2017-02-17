using System;
using Bot.Tools.Interfaces;
using Bot.Tools.Logging;

namespace Bot.Tools {
  public static class TimeExtensionMethods {

    // http://stackoverflow.com/a/7844741/625919
    public static DateTime FromUnixTime(this long unixTime) => TimeService.UnixEpoch.AddSeconds(unixTime);

      // http://stackoverflow.com/a/7844741/625919
    public static long ToUnixTime(this DateTime date) => Convert.ToInt64((date - TimeService.UnixEpoch).TotalSeconds);

      public static bool IsWithin(this DateTime test, DateTime now, TimeSpan window) =>
      (test - now).Duration() <= window;
    
    public static string ToPretty(this TimeSpan span, ILogger logger) {
      var day = Convert.ToInt32(span.ToString("%d"));
      var hour = Convert.ToInt32(span.ToString("%h"));
      var minute = Convert.ToInt32(span.ToString("%m"));

      if (span.CompareTo(TimeSpan.Zero) == -1) {
        logger.LogWarning($"Time to sync the clock?{span}");
        return "a few seconds";
      }

      if (day > 1) {
        if (hour == 0) return $"{day} days";
        return $"{day} days {hour}h";
      }

      if (day == 1) {
        if (hour == 0) return "1 day";
        return $"1 day {hour}h";
      }

      if (hour == 0) return $"{minute}m";
      if (minute == 0) return $"{hour}h";

      return $"{hour}h {minute}m";
    }

    /// <summary>
    /// Multiplies a timespan by an integer value
    /// http://stackoverflow.com/a/14285561/625919
    /// </summary>
    public static TimeSpan Multiply(this TimeSpan multiplicand, int multiplier) => TimeSpan.FromTicks(multiplicand.Ticks * multiplier);

    /// <summary>
    /// Multiplies a timespan by a double value
    /// http://stackoverflow.com/a/14285561/625919
    /// </summary>
    public static TimeSpan Multiply(this TimeSpan multiplicand, double multiplier) => TimeSpan.FromTicks((long) (multiplicand.Ticks * multiplier));
  }
}
