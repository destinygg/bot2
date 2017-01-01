using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IContextualizedProcessor {
    IReadOnlyList<ISendable> Process(IContextualized contextualized);
  }
}
