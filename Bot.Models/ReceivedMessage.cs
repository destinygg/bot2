using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public abstract class ReceivedMessage<TUser, TTransmission> : IReceived<TUser, TTransmission>
    where TUser : IUser
    where TTransmission : IMessage {
    protected ReceivedMessage(TUser sender, ITimeService timeService) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
    }

    protected ReceivedMessage(TUser sender, DateTime timestamp) {
      Timestamp = timestamp;
      Sender = sender;
    }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; }
    public TUser Sender { get; }
    public abstract TTransmission Transmission { get; }
    public string Text => Transmission.Text;
    public abstract TResult Accept<TResult>(IReceivedVisitor<TResult> visitor);
  }
}
