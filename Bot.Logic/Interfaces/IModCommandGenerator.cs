using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface IModCommandGenerator {
    IReadOnlyList<ISendable<ITransmittable>> Generate(ISnapshot<Moderator, IMessage> snapshot);
  }
}
