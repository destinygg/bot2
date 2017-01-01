using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface ISendableGenerator {
    IReadOnlyList<ISendable> Process(IContextualized contextualized);
  }
}
