using System;

namespace Bot.Models {
  public class Nuke {

    public Nuke(DateTime timestamp, TimeSpan duration, Predicate<string> matchesNukedTerm) {
      Timestamp = timestamp;
      Duration = duration;
      MatchesNukedTerm = matchesNukedTerm;
    }

    public Predicate<string> MatchesNukedTerm { get; }
    public TimeSpan Duration { get; }
    public DateTime Timestamp { get; }
  }
}
