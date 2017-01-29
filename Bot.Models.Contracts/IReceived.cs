using System;

namespace Bot.Models.Contracts {
  public interface IReceived<out T> where T : IUser {
    DateTime Timestamp { get; }
    T Sender { get; }
  }
}
