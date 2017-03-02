using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;

namespace Bot.Logic {
  public class NukeLogic : NukeAegisBase, INukeLogic {

    public IReadOnlyList<ISendable<Mute>> Nuke(IParsedNuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context) =>
      GetCurrentVictims(nuke, context)
      .Select(u => new SendableMute(u, nuke.Duration)).ToList();

  }
}
