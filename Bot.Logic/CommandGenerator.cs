using System;
using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class CommandGenerator : ICommandGenerator {

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.First as IPublicMessageReceived;
      if (message != null) {
        if (message.StartsWith("!time")) {
          outbox.Add(new SendablePublicMessage(DateTime.Now.ToShortTimeString()));
        }
      }
      return outbox;
    }
  }
}
