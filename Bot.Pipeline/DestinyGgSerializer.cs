using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;
using Newtonsoft.Json;

namespace Bot.Pipeline {
  public class DestinyGgSerializer : IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>> {

    private SendablePublicMessage _cache = new SendablePublicMessage("");

    public IEnumerable<string> Create(IEnumerable<ISendable<ITransmittable>> sendables) {
      var r = new List<ISendable<ITransmittable>>();
      sendables = sendables.ToList();
      foreach (var sendable in sendables) {
        if (sendable is SendablePublicMessage) {
          var sendablePublicMessage = (SendablePublicMessage) sendable;
          var toAdd = sendablePublicMessage.Text == _cache.Text ? new SendablePublicMessage(sendablePublicMessage.Text + ".") : sendablePublicMessage;
          r.Add(toAdd);
          _cache = toAdd;
        } else {
          r.Add(sendable);
        }
      }
      return r.Select(s => $"{s.Json.Command} {JsonConvert.SerializeObject(s.Json)}");
    }

  }
}
