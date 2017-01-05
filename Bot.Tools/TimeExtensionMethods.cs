using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public static bool IsWithin(this DateTime date, TimeSpan maxDuration) {
      var delta = date - DateTime.UtcNow;
      return delta <= maxDuration;
    }

  }
}
