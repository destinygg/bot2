using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Bot.Models.Interfaces;
using Bot.Pipeline.Interfaces;
using Bot.Tools.Interfaces;

namespace Bot.Tests {
  public class PipelineManager : IPipeline {

    private readonly TransformBlock<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>> _snapshotFactoryBlock;

    public PipelineManager(
      IErrorableFactory<string, IReceived<IUser, ITransmittable>> parser,
      IErrorableFactory<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>> snapshotFactory,
      IErrorableFactory<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>> sendableFactory,
      IFactory<IEnumerable<ISendable<ITransmittable>>, IEnumerable<string>> serializer,
      ICommandHandler<IEnumerable<string>> sender) {
      var parserBlock = new TransformBlock<string, IReceived<IUser, ITransmittable>>(s => parser.Create(s));
      _snapshotFactoryBlock = new TransformBlock<IReceived<IUser, ITransmittable>, ISnapshot<IUser, ITransmittable>>(r => snapshotFactory.Create(r));
      var sendableFactoryBlock = new TransformBlock<ISnapshot<IUser, ITransmittable>, IReadOnlyList<ISendable<ITransmittable>>>(c => sendableFactory.Create(c), new ExecutionDataflowBlockOptions {
        MaxDegreeOfParallelism = DataflowBlockOptions.Unbounded,
        EnsureOrdered = false,
      });
      var serializerBlock = new TransformBlock<IReadOnlyList<ISendable<ITransmittable>>, IEnumerable<string>>(r => serializer.Create(r));
      var senderBlock = new ActionBlock<IEnumerable<string>>(r => sender.Handle(r));

      parserBlock.LinkTo(_snapshotFactoryBlock);
      _snapshotFactoryBlock.LinkTo(sendableFactoryBlock);
      sendableFactoryBlock.LinkTo(serializerBlock);
      serializerBlock.LinkTo(senderBlock);
    }

    public void Enqueue(IReceived<IUser, ITransmittable> received) => _snapshotFactoryBlock.SendAsync(received);
  }
}
