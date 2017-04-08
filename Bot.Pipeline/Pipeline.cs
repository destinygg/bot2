using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Pipeline {
  public class Pipeline : IPipeline {
    private readonly BufferBlock<IReceived<IUser, ITransmittable>> _bufferBlock = new BufferBlock<IReceived<IUser, ITransmittable>>();
    public Pipeline(
      IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>> snapshotFactory,
      IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> sendableFactory,
      ICommandHandler<IEnumerable<ISendable<ITransmittable>>> sender) {
      var snapshotFactoryBlock = new TransformBlock<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>(r => snapshotFactory.Create(r));
      var sendableFactoryBlock = new TransformBlock<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>(c => sendableFactory.Create(c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false,
      });
      var senderBlock = new ActionBlock<IReadOnlyList<ISendable<ITransmittable>>>(r => sender.Handle(r));

      _bufferBlock.LinkTo(snapshotFactoryBlock);
      snapshotFactoryBlock.LinkTo(sendableFactoryBlock);
      sendableFactoryBlock.LinkTo(senderBlock);
    }

    public async void Run(IEnumerable<IReceived<IUser, ITransmittable>> received) {
      await SlowlyAddToBuffer(received);
    }

    private async Task SlowlyAddToBuffer(IEnumerable<IReceived<IUser, ITransmittable>> received) {
      foreach (var r in received) {
        await _bufferBlock.SendAsync(r);
        await Task.Delay(1000);
      }
    }

  }
}
