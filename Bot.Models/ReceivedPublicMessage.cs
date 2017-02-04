using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public abstract class ReceivedPublicMessage<TUser> : ReceivedMessage<TUser, PublicMessage>
    where TUser : IUser {
    protected ReceivedPublicMessage(TUser sender, string text, ITimeService timeService) : base(sender, timeService) {
      Transmission = new PublicMessage(text);
    }

    protected ReceivedPublicMessage(TUser sender, string text, DateTime timestamp) : base(sender, timestamp) {
      Transmission = new PublicMessage(text);
    }

    // To ensure thread safety, this object should remain readonly.
    public override PublicMessage Transmission { get; }
  }
}
