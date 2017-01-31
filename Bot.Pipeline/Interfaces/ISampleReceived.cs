using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Interfaces {
  public interface ISampleReceived {
    IEnumerable<IReceived<IUser, ITransmittable>> Receiveds { get; }
  }
}
