using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class Pipeline : IPipeline {
    private readonly BufferBlock<IReceived<IUser>> _bufferBlock = new BufferBlock<IReceived<IUser>>();
    public Pipeline(IReceivedToContextualized receivedToContextualized, IContextualizedToSendable contextualizedToSendable, ISender sender) {
      var receivedToContextualizedBlock = new TransformBlock<IReceived<IUser>, IContextualized>(r => receivedToContextualized.GetContextualized(r));
      var contextualizedToSendableBlock = new TransformBlock<IContextualized, IReadOnlyList<ISendable>>(c => contextualizedToSendable.GetSendables(c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false,
      });
      var senderBlock = new ActionBlock<IReadOnlyList<ISendable>>(r => sender.Send(r));

      _bufferBlock.LinkTo(receivedToContextualizedBlock);
      receivedToContextualizedBlock.LinkTo(contextualizedToSendableBlock);
      contextualizedToSendableBlock.LinkTo(senderBlock);
    }

    public async void Run(ISampleReceived sampleReceived) {
      await SlowlyAddToBuffer(sampleReceived);
    }

    private async Task SlowlyAddToBuffer(ISampleReceived sampleReceived) {
      foreach (var received in sampleReceived.Receiveds) {
        await _bufferBlock.SendAsync(received);
        await Task.Delay(1000);
      }
    }

  }
}
