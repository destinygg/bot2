using System;
using Bot.Tools;

namespace Bot.Tests {
  public class TestSettings : ISettings {

    private static readonly Settings Settings = new Settings();

    public string ClientType { get; set; } = Settings.ClientType;
    public TimeSpan ClientCheckerInterval { get; set; } = Settings.ClientCheckerInterval;
    public int ContextSize { get; set; } = Settings.ContextSize;
    public string SqlitePath { get; set; } = Settings.SqlitePath;
    public TimeSpan NukeBlastRadius { get; set; } = Settings.NukeBlastRadius;
    public TimeSpan NukeMaximumLinger { get; set; } = Settings.NukeMaximumLinger;
    public double NukeMinimumStringSimilarity { get; set; } = Settings.NukeMinimumStringSimilarity;
    public bool IsMono { get; set; } = Settings.IsMono;
    public string DestinyTimeZone { get; set; } = Settings.DestinyTimeZone;
    public string DebuggerTimeZone { get; set; } = Settings.DebuggerTimeZone;
    public double MinimumPunishmentSimilarity { get; set; } = Settings.MinimumPunishmentSimilarity;
    public TimeSpan CivilianCommandInterval { get; set; } = Settings.CivilianCommandInterval;
    public TimeSpan PeriodicSteamCheckInterval { get; set; } = Settings.PeriodicSteamCheckInterval;
    public TimeSpan PeriodicMessageInterval { get; set; } = Settings.PeriodicMessageInterval;
    public TimeSpan AutoLiveCheckInterval { get; set; } = Settings.AutoLiveCheckInterval;
    public TimeSpan OnOffTimeTolerance { get; set; } = Settings.OnOffTimeTolerance;
    public TimeSpan SelfSpamWindow { get; set; } = Settings.SelfSpamWindow;
    public TimeSpan LongSpamWindow { get; set; } = Settings.LongSpamWindow;
    public int LongSpamMinimumLength { get; set; } = Settings.LongSpamMinimumLength;
    public int RepeatCharacterSpamLimit { get; set; } = Settings.RepeatCharacterSpamLimit;
    public TimeSpan TwitterStatusUpdaterInterval { get; set; } = Settings.TwitterStatusUpdaterInterval;
    public TimeSpan MaximumBackoffTime { get; set; } = Settings.MaximumBackoffTime;
  }
}
