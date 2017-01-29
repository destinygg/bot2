using System;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class MessageFromMod : ReceivedMessage<Moderator> {
    protected MessageFromMod(Moderator sender, string text, ITimeService timeService) : base(sender, text, timeService) { }
    protected MessageFromMod(Moderator sender, string text, DateTime timestamp) : base(sender, text, timestamp) { }

  }
}
