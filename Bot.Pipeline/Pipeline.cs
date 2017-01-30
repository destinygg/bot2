using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;
using Bot.Pipeline.Contracts;

namespace Bot.Pipeline {
  public class Pipeline : IPipeline {
    private readonly BufferBlock<IReceived<IUser, ITransmittable>> _bufferBlock = new BufferBlock<IReceived<IUser, ITransmittable>>();
    public Pipeline(IReceivedToSnapshot receivedToSnapshot, ISnapshotToSendable snapshotToSendable, ISender sender) {
      var receivedToSnapshotBlock = new TransformBlock<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>(r => receivedToSnapshot.GetSnapshot(r));
      var snapshotToSendableBlock = new TransformBlock<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable>>(c => snapshotToSendable.GetSendables(c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false,
      });
      var senderBlock = new ActionBlock<IReadOnlyList<ISendable>>(r => sender.Send(r));

      _bufferBlock.LinkTo(receivedToSnapshotBlock);
      receivedToSnapshotBlock.LinkTo(snapshotToSendableBlock);
      snapshotToSendableBlock.LinkTo(senderBlock);
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
