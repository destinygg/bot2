using System.Collections.Generic;
using System.Linq;
using Bot.Models.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class TwitchSerializer : IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>> {

    public IEnumerable<string> Create(IEnumerable<ISendable<ITransmittable>> input) => input.Select(s => s.Twitch);
  }
}
