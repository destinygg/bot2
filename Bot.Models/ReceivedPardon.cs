using System;
using System.Collections.Generic;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public class ReceivedPardon : IReceived<Moderator, Pardon> {
    public ReceivedPardon(Moderator sender, Civilian target, ITimeService timeService) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
      Transmission = new Pardon(target);
    }

    public DateTime Timestamp { get; }
    public Moderator Sender { get; }
    public Pardon Transmission { get; }
    public IUser Target => Transmission.Target;
    public TResult Accept<TResult>(IReceivedVisitor<TResult> visitor) => visitor.Visit(this);
  }
}
