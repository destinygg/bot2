using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ScanForCommands : IScanForCommands {

    public IReadOnlyList<ISendable> Scan(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.First as IPublicMessageReceived;
      if (message != null) {
        if (message.StartsWith("!time")) {
          outbox.Add(new PublicMessage(DateTime.Now.ToShortTimeString()));
        }
      }
      return outbox;
    }
  }
}
