using System;

namespace Bot.Tools {
  public interface ISettings {
    int ContextSize { get; }
    string DebuggerTimeZone { get; }
    string DestinyTimeZone { get; }
    bool IsMono { get; }
    TimeSpan NukeBlastRadius { get; }
    TimeSpan NukeMaximumLinger { get; }
    double NukeMinimumStringSimilarity { get; }
    string SqlitePath { get; }
    double MinimumPunishmentSimilarity { get; }
    TimeSpan CivilianCommandInterval { get; }
    TimeSpan PeriodicTaskInterval { get; }
    TimeSpan AutoLiveCheckInterval { get; }
    TimeSpan OnOffTimeTolerance { get; }
    TimeSpan SelfSpamWindow { get; }
  }
}
