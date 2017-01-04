using System;

namespace Bot.Models.Contracts {
  public interface IReceived {
    DateTime Timestamp { get; }
    IUser Sender { get; }
    bool FromMod { get; }
  }
}
