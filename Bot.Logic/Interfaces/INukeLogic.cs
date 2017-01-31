using System.Collections.Generic;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface INukeLogic {
    IReadOnlyList<ISendable> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    IReadOnlyList<ISendable> Nuke(IParsedNuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context);
  }
}
