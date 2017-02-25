using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bot.Logic.Tests {
  public static class TimeParser {
    public static DateTime Parse(string timestamp) {
      var possibleDateTimes = new List<DateTime?> {
        _TryParseExact(timestamp, "%s.fffffff"), // https://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.85).aspx#UsingSingleSpecifiers
        _TryParseExact(timestamp, "%m"),
        _TryParseExact(timestamp, "%H:mm"),
        _TryParseExact(timestamp, "%H:mm:ss"),
        _TryParseExact(timestamp, "%H:mm:ss.fffffff")
      };
      var singleOrDefault = possibleDateTimes.SingleOrDefault(d => d != null);
      if (singleOrDefault != null) return (DateTime) singleOrDefault;
      throw new Exception("Invalid DateTime format.");
    }

    private static DateTime? _TryParseExact(string timestamp, string format) {
      DateTime outTime;
      return DateTime.TryParseExact(timestamp, format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.NoCurrentDateDefault, out outTime)
        ? (DateTime?) outTime
        : null;
    }

  }
}
