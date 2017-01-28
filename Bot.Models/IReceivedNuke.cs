using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public interface IReceivedNuke : IReceived {
    TimeSpan Duration { get; }
    bool WillPunish<T>(T message) where T : IReceived, IMessage;

  }
}
