using System;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class ReceivedMessageFromMod : ReceivedMessage {
    protected ReceivedMessageFromMod(Moderator sender, string text, ITimeService timeService) : base(sender, text, timeService) { }
    protected ReceivedMessageFromMod(Moderator sender, string text, DateTime timestamp) : base(sender, text, timestamp) { }

  }
}
