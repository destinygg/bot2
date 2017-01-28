using System;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class ReceivedMessage : Message, IReceived<IUser> {
    protected ReceivedMessage(IUser sender, string text, ITimeService timeService) : base(text) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
    }

    protected ReceivedMessage(IUser sender, string text, DateTime timestamp) : base(text) {
      Timestamp = timestamp;
      Sender = sender;
    }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; }
    public IUser Sender { get; }

  }
}
