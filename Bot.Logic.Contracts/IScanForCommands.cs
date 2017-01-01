using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScanForCommands {
    IReadOnlyList<ISendable> Scan(IContextualized contextualized);
  }
}
