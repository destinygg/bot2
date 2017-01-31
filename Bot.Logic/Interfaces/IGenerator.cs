using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IGenerator {
    IReadOnlyList<ISendable> Generate(ISnapshot<IUser, ITransmittable> snapshot);
  }
}
