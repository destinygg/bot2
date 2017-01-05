using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Models;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SampleReceivedProducer : IReceivedProducer {
    private readonly IEnumerable<IReceived> _received;
    private readonly BufferBlock<IReceived> _producer;

    public SampleReceivedProducer() {
      _received = new List<IReceived> {
        new ModPublicReceivedMessage("!long"),
        new PublicReceivedMessage("hi"),
        new PublicReceivedMessage("banplox"),
        new PublicReceivedMessage("!time"),
        new ModPublicReceivedMessage("!sing"),
        new ModPublicReceivedMessage("!long"),
      };
      _producer = new BufferBlock<IReceived>();
      Run();
    }

    private void Run() {
      foreach (var received in _received) {
        _producer.Post(received);
      }
    }

    public ISourceBlock<IReceived> ReceivedBlock => _producer;

  }
}
