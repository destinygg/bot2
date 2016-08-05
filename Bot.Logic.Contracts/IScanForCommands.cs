using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScanForCommands {
    IEnumerable<ISendable> Scan(IPublicMessageReceived message);
  }
}
