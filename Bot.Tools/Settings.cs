using System;

namespace Bot.Tools {
  public static class Settings {
    public const int ContextSize = 1000;
    public const string SqlitePath = "Bot.sqlite";
    public static TimeSpan NukeBlastRadius => TimeSpan.FromMinutes(5);
    public const double NukeMinimumStringSimilarity = 0.7;
  }
}
