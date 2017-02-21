using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models.Received {
  public class ReceivedError : ReceivedMessage<Moderator, ErrorMessage> {
    public ReceivedError(string text, ITimeService timeService) : base(new Moderator("Internal Bot System"), timeService) {
      Transmission = new ErrorMessage(text);
    }

    public ReceivedError(string text, DateTime timestamp) : base(new Moderator("Internal Bot System"), timestamp) {
      Transmission = new ErrorMessage(text);
    }

    public override ErrorMessage Transmission { get; }
    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
