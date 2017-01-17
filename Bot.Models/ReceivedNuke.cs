using System;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class ReceivedNuke : IReceived {
    protected ReceivedNuke(ReceivedMessage message, ITimeService timeService) {
      Sender = message.Sender;
      Timestamp = timeService.UtcNow;
    }

    public abstract TimeSpan Duration { get; }
    public abstract bool IsMatch(string victimText);

    public DateTime Timestamp { get; }
    public IUser Sender { get; }
  }
}
