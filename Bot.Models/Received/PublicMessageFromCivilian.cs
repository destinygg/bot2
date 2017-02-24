using System;
using System.Diagnostics;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models.Received {
  [DebuggerDisplay("{Sender}: {Text}")]
  public class PublicMessageFromCivilian : ReceivedPublicMessage<Civilian> {
    public PublicMessageFromCivilian(string text, ITimeService timeService)
      : base(new Civilian("SampleUser"), text, timeService) { }

    public PublicMessageFromCivilian(string text, DateTime timestamp)
      : base(new Civilian(timestamp.ToShortTimeString()), text, timestamp) { }

    public PublicMessageFromCivilian(string nick, string text, DateTime timestamp)
      : base(new Civilian(nick), text, timestamp) { }

    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
