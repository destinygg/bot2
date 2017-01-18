using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class BanGenerator : IBanGenerator {

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.First as ReceivedMessage;
      if (message == null) return outbox;
      if (message.Text.Contains("banplox")) {
        outbox.Add(new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Text}"));
      }
      return outbox;
    }
  }
}
