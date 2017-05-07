using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;
using Newtonsoft.Json;

namespace Bot.Pipeline {
  public class DestinyGgSerializer : IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>> {

    public IEnumerable<string> Create(IEnumerable<ISendable<ITransmittable>> input) =>
      input.Select(s => $"{s.Json.Command} {JsonConvert.SerializeObject(s.Json)}");

  }
}
