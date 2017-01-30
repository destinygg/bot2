using System.Collections.Generic;
using Bot.Models.Contracts;

namespace Bot.Logic.Contracts {
  public interface IGenerator {
    IReadOnlyList<ISendable> Generate(ISnapshot<IUser, ITransmittable> snapshot);
  }
}
