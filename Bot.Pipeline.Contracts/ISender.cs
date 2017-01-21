using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface ISender {
    void Send(IEnumerable<ISendable> sendables);
  }
}
