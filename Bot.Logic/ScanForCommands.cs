using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class ScanForCommands : IScanForCommands {
    public IEnumerable<ISendable> Scan(IPublicMessageReceived message) {
      var outbox = new List<ISendable>();
      if (message.Text.Contains("!time"))
        outbox.Add(new PublicMessage(DateTime.Now.ToShortTimeString()));
      return outbox;
    }
  }
}
