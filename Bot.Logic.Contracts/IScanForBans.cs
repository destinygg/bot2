﻿using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScanForBans {
    IEnumerable<ISendable> Scan(IPublicMessageReceived message);
  }
}