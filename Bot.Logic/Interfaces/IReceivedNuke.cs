using System;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IReceivedNuke : IReceived<IUser, IMessage> {
    TimeSpan Duration { get; }
    bool WillPunish(IReceivedMessage<Civilian> message);

  }
}
