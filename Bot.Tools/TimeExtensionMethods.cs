using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Pipeline.Contracts;

namespace Bot.Tools {
  public static class TimeExtensionMethods {

    // http://stackoverflow.com/a/7844741/625919
    public static DateTime FromUnixTime(this long unixTime) {
      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return epoch.AddSeconds(unixTime);
    }

    // http://stackoverflow.com/a/7844741/625919
    public static long ToUnixTime(this DateTime date) {
      var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return Convert.ToInt64((date - epoch).TotalSeconds);
    }

    public static bool IsBeforeAndWithin(this DateTime beforeAndWithin, TimeSpan window)
      => DateTime.UtcNow - window.Duration() <= beforeAndWithin && beforeAndWithin <= DateTime.UtcNow;

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
