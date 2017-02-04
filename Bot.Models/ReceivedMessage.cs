using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public abstract class ReceivedMessage<TUser, TTransmission> : Received<TUser, TTransmission>
    where TUser : IUser
    where TTransmission : IMessage {
    protected ReceivedMessage(TUser sender, ITimeService timeService) : base(timeService.UtcNow, sender) { }

    protected ReceivedMessage(TUser sender, DateTime timestamp) : base(timestamp, sender) { }

    // To ensure thread safety, this object should remain readonly.
    public string Text => Transmission.Text;
  }
}
