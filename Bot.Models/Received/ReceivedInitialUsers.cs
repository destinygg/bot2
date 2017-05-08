using System;
using Bot.Models.Interfaces;

namespace Bot.Models.Received {
  public class ReceivedInitialUsers : ReceivedFromSystem<InitialUsers> {
    public ReceivedInitialUsers(DateTime timestamp, InitialUsers initialUsers) : base(timestamp) {
      Transmission = initialUsers;
    }

    public override InitialUsers Transmission { get; }
    public override TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
