using System;
using System.Diagnostics;
using Bot.Tools.Contracts;

namespace Bot.Models {
  [DebuggerDisplay("From:{Sender.Nick} Saying:{Text}")]
  public class PublicReceivedMessageFromMod : ReceivedMessageFromMod {
    public PublicReceivedMessageFromMod(string text, ITimeService timeService) : base(new Moderator("SampleMod"), text, timeService) { }
    public PublicReceivedMessageFromMod(string text, DateTime timestamp) : base(new Moderator(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
