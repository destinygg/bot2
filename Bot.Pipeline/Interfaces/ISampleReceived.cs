using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Pipeline.Interfaces {
  public interface ISampleReceived {
    IEnumerable<IReceived<IUser, ITransmittable>> Receiveds { get; }
  }
}
