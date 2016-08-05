using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  interface IScanForModCommands {
    IEnumerable<ISendable> Scan(IPublicMessageReceived message);
  }
}
