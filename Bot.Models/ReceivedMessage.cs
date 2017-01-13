using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public abstract class ReceivedMessage : Message, IReceived {
    protected ReceivedMessage(string text) : base(text) { }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; protected set; }
    public IUser Sender { get; protected set; }

  }
}
