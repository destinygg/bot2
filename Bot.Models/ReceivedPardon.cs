using System;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedPardon : Pardon, IReceived<Moderator> {
    public ReceivedPardon(Moderator sender, Civilian target, ITimeService timeService) : base(target) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
    }

    public DateTime Timestamp { get; }
    public Moderator Sender { get; }
  }
}
