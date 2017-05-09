using System;
using Bot.Models.Interfaces;

namespace Bot.Models.Received {
  public class ReceivedQuit : Received<IUser, Quit> {

    public ReceivedQuit(IUser sender, DateTime timestamp) : base(timestamp, sender) { }

    public override Quit Transmission { get; }

    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
