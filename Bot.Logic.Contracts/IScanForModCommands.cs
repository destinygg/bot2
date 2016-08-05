using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScanForModCommands {
    IEnumerable<ISendable> Scan(IMessageReceived message);
  }
}
