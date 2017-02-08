using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IBanGenerator {
    IReadOnlyList<ISendable<ITransmittable>> Generate(ISnapshot<Civilian, PublicMessage> snapshot);
  }
}
