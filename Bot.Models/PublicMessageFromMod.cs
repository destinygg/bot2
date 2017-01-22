using System;
using System.Diagnostics;
using Bot.Tools.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class PublicMessageFromMod : MessageFromMod {
    public PublicMessageFromMod(string text, ITimeService timeService) : base(new Moderator("SampleMod"), text, timeService) { }
    public PublicMessageFromMod(string text, DateTime timestamp) : base(new Moderator(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
