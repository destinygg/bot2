using System.Threading.Tasks.Dataflow;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface ISendableProducer {
    ISourceBlock<ISendable> Produce { get; }
  }
}
