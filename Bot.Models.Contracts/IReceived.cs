﻿using System;

namespace Bot.Models.Contracts {
  public interface IReceived {
    DateTime Received { get; }
    string Sender { get; }
  }
}
