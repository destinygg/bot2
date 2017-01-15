using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public class ReceivedUnMuteBan : UnMuteBan, IReceived {
    public ReceivedUnMuteBan(IUser sender, IUser target) : base(target) {
      Sender = sender;
    }

    public DateTime Timestamp { get; } = DateTime.UtcNow;
    public IUser Sender { get; }
  }
}
