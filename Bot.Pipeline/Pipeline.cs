using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class Pipeline : IPipeline {
    private readonly BufferBlock<IReceived<IUser, ITransmittable>> _bufferBlock = new BufferBlock<IReceived<IUser, ITransmittable>>();
    public Pipeline(IReceivedToContextualized receivedToContextualized, IContextualizedToSendable contextualizedToSendable, ISender sender) {
      var receivedToContextualizedBlock = new TransformBlock<IReceived<IUser, ITransmittable>, IContextualized<IUser, ITransmittable>>(r => receivedToContextualized.GetContextualized(r));
      var contextualizedToSendableBlock = new TransformBlock<IContextualized<IUser, ITransmittable>, IReadOnlyList<ISendable>>(c => contextualizedToSendable.GetSendables(c), new ExecutionDataflowBlockOptions {
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
