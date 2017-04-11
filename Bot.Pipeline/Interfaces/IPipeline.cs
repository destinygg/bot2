using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Pipeline.Interfaces {
  public interface IPipeline {
    void Enqueue(IReceived<IUser, ITransmittable> received);
  }
}
