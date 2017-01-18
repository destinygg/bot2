using System;

namespace Bot.Models.Contracts {
  public interface IReceivedNuke : IReceived {
    TimeSpan Duration { get; }
    bool WillPunish<T>(T message) where T : IReceived, IMessage;

  }
}
