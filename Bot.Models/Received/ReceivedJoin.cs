using System;
using Bot.Models.Interfaces;

namespace Bot.Models.Received {
  public class ReceivedJoin : Received<IUser, Join> {

    public ReceivedJoin(IUser sender, DateTime timestamp) : base(timestamp, sender) { }

    public override Join Transmission { get; }

    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
