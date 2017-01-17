using System;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class ReceivedNuke : IReceivedNuke {
    protected ReceivedNuke(ReceivedMessage message, ITimeService timeService) {
      Sender = message.Sender;
      Timestamp = timeService.UtcNow;
    }

    public abstract TimeSpan Duration { get; }
    public abstract bool IsMatch(string possibleVictimText);

    public DateTime Timestamp { get; }
    public IUser Sender { get; }
  }
}
