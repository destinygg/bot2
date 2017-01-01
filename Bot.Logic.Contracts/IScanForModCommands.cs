using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScanForModCommands {
    IReadOnlyList<ISendable> Scan(IContextualized contextualized);
  }
}
