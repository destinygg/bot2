using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;

namespace Bot.Logic {
  public abstract class NukeAegisBase {
    protected IEnumerable<Civilian> GetCurrentVictims(IParsedNuke nuke, IEnumerable<IReceived<IUser, ITransmittable>> context) => context
      .OfType<IReceived<Civilian, PublicMessage>>()
      .Where(nuke.WillPunish)
      .Select(m => m.Sender)
      .Distinct();
  }
}
