using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Pipeline.Contracts {
  public interface IContextualizedToSendable {
    IReadOnlyList<ISendable> GetSendables(IContextualized<IUser, ITransmittable> contextualized);
  }
}
