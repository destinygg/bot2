using System.Collections.Generic;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic.Interfaces {
  public interface INukeLogic {
    IReadOnlyList<ISendable<Pardon>> Aegis(IReadOnlyList<IReceived<IUser, ITransmittable>> context);
    IReadOnlyList<ISendable<Mute>> Nuke(IParsedNuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context);
  }
}
