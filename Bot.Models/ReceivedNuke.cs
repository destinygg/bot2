using System;
using Bot.Models.Contracts;
using Bot.Tools;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class ReceivedNuke : IReceivedNuke {

    protected ReceivedNuke(ReceivedMessage message, ITimeService timeService) {
      Timestamp = timeService.UtcNow;
      Sender = message.Sender;
    }

    public bool WillPunish<T>(T message) where T : IReceived, IMessage =>
      !message.FromMod() &&
      WillPunish(message.Text) &&
      WithinRange(message);

    private bool WithinRange<T>(T message) where T : IReceived =>
      message.Timestamp >= Timestamp - Settings.NukeBlastRadius &&
      message.Timestamp <= Timestamp + Settings.NukeBlastRadius;

    protected abstract bool WillPunish(string possibleVictimText);
    public abstract TimeSpan Duration { get; }
    public DateTime Timestamp { get; }
    public IUser Sender { get; }

  }
}
