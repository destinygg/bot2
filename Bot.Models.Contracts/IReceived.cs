using System;

namespace Bot.Models.Contracts {
  public interface IReceived<T> where T : IUser {
    DateTime Timestamp { get; }
    T Sender { get; }
  }
}
