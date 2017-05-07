using System.Collections.Generic;
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
      IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>> serializer,
      ICommandHandler<IEnumerable<string>> sender) {
      var snapshotFactoryBlock = new TransformBlock<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>(r => snapshotFactory.Create(r));
      var sendableFactoryBlock = new TransformBlock<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>(c => sendableFactory.Create(c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false,
      });
      var serializerBlock = new TransformBlock<IReadOnlyList<ISendable<ITransmittable>>, IEnumerable<string>>(r => serializer.Create(r));
      var senderBlock = new ActionBlock<IEnumerable<string>>(r => sender.Handle(r));

      _bufferBlock.LinkTo(snapshotFactoryBlock);
      snapshotFactoryBlock.LinkTo(sendableFactoryBlock);
      sendableFactoryBlock.LinkTo(serializerBlock);
      serializerBlock.LinkTo(senderBlock);
    }

    public void Enqueue(IReceived<IUser, ITransmittable> received) => _bufferBlock.SendAsync(received);
  }
}
