using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SampleReceived : ISampleReceived {
    private readonly IReceivedFactory _factory;

    public SampleReceived(IReceivedFactory factory) {
      _factory = factory;
    }

    public IEnumerable<IReceived<IUser>> Receiveds => new List<IReceived<IUser>> {
        _factory.ModPublicReceivedMessage("!long"),
        _factory.PublicReceivedMessage("hi"),
        _factory.PublicReceivedMessage("banplox"),
        _factory.PublicReceivedMessage("!time"),
        _factory.ModPublicReceivedMessage("!sing"),
        _factory.ModPublicReceivedMessage("!long"),
      };
  }
}
