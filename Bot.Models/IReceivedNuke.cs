﻿using System;
using Bot.Models.Contracts;

namespace Bot.Models {
  public interface IReceivedNuke : IReceived<IUser> {
    TimeSpan Duration { get; }
    bool WillPunish<T>(T message) where T : IReceived<IUser>, IMessage;

  }
}
