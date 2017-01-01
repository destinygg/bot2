using System.Collections.Generic;

namespace Bot.Models.Contracts {
  public interface IContextualized {

    /// <summary>
    /// The first Received message
    /// </summary>
    IReceived First { get; }

    /// <summary>
    /// The rest of the Received messages; doesn't include First
    /// </summary>
    IReadOnlyList<IReceived> Context { get; }
  }
}
