﻿using System;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic.Interfaces {
  public interface IReceivedNuke : IReceived<IUser, IMessage> {
    TimeSpan Duration { get; }
    bool WillPunish(IReceivedMessage<Civilian> message);

  }
}