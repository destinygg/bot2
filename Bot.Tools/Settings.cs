using System;

namespace Bot.Tools {
  public static class Settings {
    public const int ContextSize = 1000;
    public const string SqlitePath = "Bot.sqlite";
    public static TimeSpan DefaultNukeBlastRadius => TimeSpan.FromMinutes(5);
    public static TimeSpan AegisRadiusAroundFirstNuke => DefaultNukeBlastRadius.Multiply(2);
    public const double NukeMinimumStringSimilarity = 0.7;
  }
}
