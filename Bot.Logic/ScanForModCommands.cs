using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {

  public class ScanForModCommands : IScanForModCommands {
    public IEnumerable<ISendable> Scan(IPublicMessageReceived message) {
      var outbox = new List<ISendable>();
      if (message.Text.Contains("!sing"))
        outbox.Add(new PublicMessage("/me sings a song"));
      return outbox;
    }
  }
}
