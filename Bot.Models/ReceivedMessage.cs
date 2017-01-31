using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public abstract class ReceivedMessage<T> : Message, IReceivedMessage<T>
    where T : IUser {
    protected ReceivedMessage(T sender, string text, ITimeService timeService) : base(text) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
    }

    protected ReceivedMessage(T sender, string text, DateTime timestamp) : base(text) {
      Timestamp = timestamp;
      Sender = sender;
    }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; }
    public T Sender { get; }
    public IMessage Transmission => this;

  }
}
