using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Pipeline.Interfaces {
  public interface ISender {
    void Send(IEnumerable<ISendable<ITransmittable>> sendables);
  }
}
