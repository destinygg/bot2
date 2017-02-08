using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface ICommandGenerator {
    IReadOnlyList<ISendable<ITransmittable>> Generate(ISnapshot<IUser, IMessage> snapshot);
  }
}
