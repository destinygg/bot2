using System;

namespace Bot.Logic.Tests.Helper {
  public static class DateTimeZero {
    public static DateTime AddTicks(int ticks) => DateTime.MinValue + TimeSpan.FromTicks(ticks);
    public static DateTime AddSeconds(int seconds) => DateTime.MinValue + TimeSpan.FromSeconds(seconds);
    public static DateTime AddMinutes(int minutes) => DateTime.MinValue + TimeSpan.FromMinutes(minutes);
    public static DateTime AddHours(int hours) => DateTime.MinValue + TimeSpan.FromHours(hours);
  }
}
