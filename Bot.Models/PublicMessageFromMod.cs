using System;
using System.Diagnostics;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("{Sender}: {Text}")]
  public class PublicMessageFromMod : ReceivedPublicMessage<Moderator> {
    public PublicMessageFromMod(string text, ITimeService timeService)
      : base(new Moderator("SampleMod"), text, timeService) { }

    public PublicMessageFromMod(string text, DateTime timestamp)
      : base(new Moderator(timestamp.ToShortTimeString()), text, timestamp) { }

  }
}
