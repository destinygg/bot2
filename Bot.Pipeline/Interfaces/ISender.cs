using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Interfaces {
  public interface ISender {
    void Send(IEnumerable<ISendable> sendables);
  }
}
