﻿using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IReceivedToSnapshot {
    ISnapshot<IUser, ITransmittable> GetSnapshot(IReceived<IUser, ITransmittable> received);
  }
}
