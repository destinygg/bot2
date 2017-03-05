using System;

namespace Bot.Logic {
  public class ParsedNuke {

    public ParsedNuke(DateTime timestamp, TimeSpan duration, Predicate<string> matchesNukedTerm) {
      Timestamp = timestamp;
      Duration = duration;
      MatchesNukedTerm = matchesNukedTerm;
    }

    public Predicate<string> MatchesNukedTerm { get; }
    public TimeSpan Duration { get; }
    public DateTime Timestamp { get; }
  }
}
