using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IContextualizedProducer {
    ISourceBlock<IContextualized> Produce { get; }
  }
}
