using System;
using System.Diagnostics;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("{Sender}: {Text}")]
  public class PublicMessageFromMod : ReceivedMessage<Moderator, PublicMessage> {
    public PublicMessageFromMod(string text, ITimeService timeService) : base(new Moderator("SampleMod"), new PublicMessage(text), timeService) { }
    public PublicMessageFromMod(string text, DateTime timestamp) : base(new Moderator(timestamp.ToShortTimeString()), new PublicMessage(text), timestamp) { }

  }
}
