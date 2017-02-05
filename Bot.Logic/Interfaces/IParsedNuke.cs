﻿using System;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IParsedNuke {
    TimeSpan Duration { get; }
    bool WillPunish(IReceived<Civilian, PublicMessage> message);

  }
}