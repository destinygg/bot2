using System;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Models {
  public class ReceivedPardon : Pardon, IReceived<Moderator, Pardon> {
    public ReceivedPardon(Moderator sender, Civilian target, ITimeService timeService) : base(target) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
    }

    public DateTime Timestamp { get; }
    public Moderator Sender { get; }
    public Pardon Transmission => this;
  }
}
