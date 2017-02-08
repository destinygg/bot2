using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface ISendableGenerator {
    IReadOnlyList<ISendable<ITransmittable>> Generate(ISnapshot<IUser, ITransmittable> snapshot);
  }
}
