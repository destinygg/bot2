using System.Collections.Generic;
using Bot.Logic.Contracts;
using Bot.Models;
using Bot.Models.Contracts;

namespace Bot.Logic {
  public class BanGenerator : IBanGenerator {

    public IReadOnlyList<ISendable> Generate(IContextualized contextualized) {
      var outbox = new List<ISendable>();
      var message = contextualized.Latest as ReceivedMessage;
      if (message == null) return outbox;
      if (message.Text.Contains("banplox")) {
        outbox.Add(new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Text}"));
      }
      return outbox;
    }

    //IReadOnlyCollection?
    //public IReadOnlyList<ISendable> Generate(IContextualized contextualized) =>
    //  Herpderp(contextualized).ToList();

    //private IEnumerable<ISendable> Herpderp(IContextualized contextualized) {
    //  var message = contextualized.First as ReceivedMessage;
    //  if (message != null && message.Text.Contains("banplox")) {
    //    yield return new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Text}");
    //  }
    //}

  }
}
