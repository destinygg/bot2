using System;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public abstract class MessageFromCivilian : ReceivedMessage<Civilian> {
    protected MessageFromCivilian(Civilian sender, string text, ITimeService timeService) : base(sender, text, timeService) { }
    protected MessageFromCivilian(Civilian sender, string text, DateTime timestamp) : base(sender, text, timestamp) { }

  }
}
