using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models.Received {
  public abstract class ReceivedPrivateMessage<TUser> : ReceivedMessage<TUser, PrivateMessage>
    where TUser : IUser {
    protected ReceivedPrivateMessage(TUser sender, string text, ITimeService timeService) : base(sender, timeService) {
      Transmission = new PrivateMessage(text, new Moderator("Bot"));
    }

    protected ReceivedPrivateMessage(TUser sender, string text, DateTime timestamp) : base(sender, timestamp) {
      Transmission = new PrivateMessage(text, new Moderator("Bot"));
    }

    // To ensure thread safety, this object should remain readonly.
    public override PrivateMessage Transmission { get; }
  }
}
