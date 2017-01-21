using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface ISampleReceived {
    IEnumerable<IReceived> Receiveds { get; }
  }
}
