using System.Collections.Generic;

namespace Bot.Models.Contracts {
  public interface IContextualized {

    /// <summary> 
    /// The latest Received transmission
    /// </summary>
    IReceived Latest { get; }

    /// <summary>
    /// The rest of the Received transmissions; doesn't include Latest
    /// </summary>
    IReadOnlyList<IReceived> Context { get; }
  }
}
