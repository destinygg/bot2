using System;
using Bot.Models.Contracts;
using Bot.Tools.Contracts;

namespace Bot.Models {
  public class ReceivedPardon : Pardon, IReceived {
    public ReceivedPardon(IUser sender, IUser target, ITimeService timeService) : base(target) {
      Timestamp = timeService.UtcNow;
      Sender = sender;
    }

    public DateTime Timestamp { get; }
    public IUser Sender { get; }
  }
}
