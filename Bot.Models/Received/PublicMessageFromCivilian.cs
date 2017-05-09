using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models.Received {
  public class PublicMessageFromCivilian : ReceivedPublicMessage<Civilian> {

    public PublicMessageFromCivilian(string text, ITimeService timeService, bool isPunishable = true)
      : base(new Civilian("SampleUser", isPunishable), text, timeService) { }

    public PublicMessageFromCivilian(string text, DateTime timestamp, bool isPunishable = true)
      : base(new Civilian(timestamp.ToShortTimeString(), isPunishable), text, timestamp) { }

    public PublicMessageFromCivilian(string nick, string text, DateTime timestamp, bool isPunishable = true)
      : base(new Civilian(nick, isPunishable), text, timestamp) { }

    public PublicMessageFromCivilian(string nick, string text, ITimeService timeService, bool isPunishable = true)
      : base(new Civilian(nick, isPunishable), text, timeService) { }

    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
    public override string ToString() => $"{Sender}: {Text}";
  }
}
