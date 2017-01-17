using System;

namespace Bot.Models.Contracts {
  public interface IReceivedNuke : IReceived {
    TimeSpan Duration { get; }
    bool IsMatch(string possibleVictimText);

  }
}
