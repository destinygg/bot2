using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class ReceivedMessage : Message, IReceived {
    protected ReceivedMessage(IUser sender, string text) : base(text) {
      Sender = sender;
    }

    protected ReceivedMessage(IUser sender, string text, DateTime timestamp) : base(text) {
      Sender = sender;
      Timestamp = timestamp;
    }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public IUser Sender { get; }

  }
}
