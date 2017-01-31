using System.Collections.Generic;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;

namespace Bot.Pipeline {
  public class SampleReceived : ISampleReceived {
    private readonly IReceivedFactory _factory;

    public SampleReceived(IReceivedFactory factory) {
      _factory = factory;
    }

    public IEnumerable<IReceived<IUser, ITransmittable>> Receiveds => new List<IReceived<IUser, ITransmittable>> {
        _factory.ModPublicReceivedMessage("!long"),
        _factory.PublicReceivedMessage("hi"),
        _factory.PublicReceivedMessage("banplox"),
        _factory.PublicReceivedMessage("!time"),
        _factory.ModPublicReceivedMessage("!sing"),
        _factory.ModPublicReceivedMessage("!long"),
      };
  }
}
