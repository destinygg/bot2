using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public class BanGenerator : IBanGenerator {

    public IReadOnlyList<ISendable> Generate(ISnapshot<IUser, ITransmittable> snapshot) {
      var outbox = new List<ISendable>();
      var message = snapshot.Latest as ReceivedMessage<Civilian>;
      if (message == null) return outbox;
      if (message.Text.Contains("banplox")) {
        outbox.Add(new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Text}"));
      }
      return outbox;
    }

    //IReadOnlyCollection?
    //public IReadOnlyList<ISendable> Generate(ISnapshot snapshot) =>
    //  Herpderp(snapshot).ToList();

    //private IEnumerable<ISendable> Herpderp(ISnapshot snapshot) {
    //  var message = snapshot.First as ReceivedMessage;
    //  if (message != null && message.Text.Contains("banplox")) {
    //    yield return new SendablePublicMessage($"{message.Sender.Nick} banned for saying {message.Text}");
    //  }
    //}

  }
}
