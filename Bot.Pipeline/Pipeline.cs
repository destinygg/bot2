using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class Pipeline : IPipeline {
    private readonly BufferBlock<IReceived<IUser, ITransmittable>> _bufferBlock = new BufferBlock<IReceived<IUser, ITransmittable>>();
    public Pipeline(IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>> snapshotFactory, IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> sendableFactory, ISender sender) {
      var snapshotFactoryBlock = new TransformBlock<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>(r => snapshotFactory.Create(r));
      var sendableFactoryBlock = new TransformBlock<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>(c => sendableFactory.Create(c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false,
      });
      var senderBlock = new ActionBlock<IReadOnlyList<ISendable<ITransmittable>>>(r => sender.Send(r));

      _bufferBlock.LinkTo(snapshotFactoryBlock);
      snapshotFactoryBlock.LinkTo(sendableFactoryBlock);
      sendableFactoryBlock.LinkTo(senderBlock);
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
