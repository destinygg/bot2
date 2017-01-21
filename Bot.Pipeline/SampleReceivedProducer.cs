using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class SampleReceivedProducer : IReceivedProducer {

    public async void Run(ISampleReceived sampleReceived) {
      foreach (var received in sampleReceived.Receiveds) {
        await Task.Delay(1000);
        await AppendToBuffer(received);
      }
    }

    private async Task AppendToBuffer(IReceived message) {
      ReceivedBlock.Post(message);
      await ReceivedBlock.SendAsync(message);
    }

    public BufferBlock<IReceived> ReceivedBlock { get; } = new BufferBlock<IReceived>();

  }
}
