using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Pipeline.Interfaces {
  public interface IPipeline {
    void Run(IEnumerable<IReceived<IUser, ITransmittable>> received);
  }
}
