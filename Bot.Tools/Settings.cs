using System;

namespace Bot.Tools {
  public class Settings : ISettings {
    public string ClientType { get; set; }
    public TimeSpan ClientCheckerInterval => PeriodicMessageInterval;
    public int ContextSize => 1000;
    public string SqlitePath => "Bot.sqlite";
    public TimeSpan NukeBlastRadius => TimeSpan.FromMinutes(5);
    public TimeSpan NukeMaximumLinger => TimeSpan.FromMinutes(10);
    public double NukeMinimumStringSimilarity => 0.7;
    public bool IsMono => Type.GetType("Mono.Runtime") != null;
    public string DestinyTimeZone => IsMono ? "US/Central" : "Central Standard Time"; // http://mono.1490590.n4.nabble.com/Cross-platform-time-zones-td1507630.html
    public string DebuggerTimeZone => IsMono ? "US/Central" : "Central Standard Time";
    public double MinimumPunishmentSimilarity => 0.7d;
    public TimeSpan CivilianCommandInterval => TimeSpan.FromSeconds(10);
    public TimeSpan PeriodicSteamCheckInterval => TimeSpan.FromMinutes(1);
    public TimeSpan PeriodicMessageInterval => TimeSpan.FromMinutes(10);
    public TimeSpan AutoLiveCheckInterval => TimeSpan.FromMinutes(1);
    public TimeSpan OnOffTimeTolerance => TimeSpan.FromMinutes(10);
    public TimeSpan SelfSpamWindow => TimeSpan.FromSeconds(15);
    public TimeSpan LongSpamWindow => TimeSpan.FromMinutes(1);
    public int LongSpamMinimumLength => 60;
    public int RepeatCharacterSpamLimit => 45;
    public TimeSpan TwitterStatusUpdaterInterval => TimeSpan.FromSeconds(15);
    public TimeSpan MaximumBackoffTime => TimeSpan.FromMinutes(1);
  }
}
