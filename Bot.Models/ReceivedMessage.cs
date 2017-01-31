using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public abstract class ReceivedMessage<TUser, TTransmission> : IReceived<TUser, TTransmission>
    where TUser : IUser
    where TTransmission : ITransmittable, IMessage {
    protected ReceivedMessage(TUser sender, TTransmission message, ITimeService timeService) {
      Timestamp = timeService.UtcNow;
      Transmission = message;
      Sender = sender;
    }

    protected ReceivedMessage(TUser sender, TTransmission message, DateTime timestamp) {
      Timestamp = timestamp;
      Transmission = message;
      Sender = sender;
    }

    // To ensure thread safety, this object should remain readonly.
    public DateTime Timestamp { get; }
    public TUser Sender { get; }
    public TTransmission Transmission { get; }

  }
}
