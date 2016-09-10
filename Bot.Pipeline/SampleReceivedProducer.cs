using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SampleReceivedProducer : IReceivedProducer {
    private readonly IEnumerable<IReceived> _received;
    private readonly BufferBlock<IReceived> _producer;

    public SampleReceivedProducer(IEnumerable<IReceived> received) {
      _received = received;
      _producer = new BufferBlock<IReceived>();
      Run();
    }

    private void Run() {
      foreach (var received in _received) {
        _producer.Post(received);
      }
    }

    public ISourceBlock<IReceived> Produce => _producer;

  }
}
