using System;
using System.Diagnostics;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  [DebuggerDisplay("{Sender}: {Text}")]
  public class PublicMessageFromMod : ReceivedPublicMessage<Moderator> {
    public PublicMessageFromMod(string text, ITimeService timeService)
      : base(new Moderator("SampleMod"), text, timeService) { }

    public PublicMessageFromMod(string text, DateTime timestamp)
      : base(new Moderator(timestamp.ToShortTimeString()), text, timestamp) { }

    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
