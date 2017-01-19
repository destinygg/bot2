using System;

namespace Bot.Tools {
  public static class Settings {
    public const int ContextSize = 1000;
    public const string SqlitePath = "Bot.sqlite";
    public static TimeSpan NukeBlastRadius => TimeSpan.FromMinutes(5);
    public const double NukeMinimumStringSimilarity = 0.7;
    public static bool IsMono = Type.GetType("Mono.Runtime") != null;
    public static string DestinyTimeZone = IsMono ? "US/Central" : "Central Standard Time"; // http://mono.1490590.n4.nabble.com/Cross-platform-time-zones-td1507630.html
    public static string DebuggerTimeZone = IsMono ? "US/Central" : "Central Standard Time";
  }
}
