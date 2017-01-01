using Bot.Models.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bot.Logic.Contracts {
  public interface IContextualizedProcessor {
    IReadOnlyList<ISendable> Process(IContextualized contextualized);
  }
}
