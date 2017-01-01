using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IScan {
    IReadOnlyList<ISendable> Scan(IContextualized contextualized);
  }
}
