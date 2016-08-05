using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ScanForBans : IScanForBans {

    public IEnumerable<ISendable> Scan(IPublicMessageReceived message) {
      var outbox = new List<ISendable>();
      if (message.Text.Contains("banplox")) {
        outbox.Add(new PublicMessage($"{message.Sender.Nick} banned for saying {message.Text}"));
      }
      return outbox;
    }
  }
}
