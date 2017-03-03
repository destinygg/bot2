using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class NukeMuteFactory : NukeAegisBase, IErrorableFactory<IParsedNuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<Mute>>> {


    public IReadOnlyList<ISendable<Mute>> Create(IParsedNuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context) =>
      GetCurrentVictims(nuke, context)
      .Select(u => new SendableMute(u, nuke.Duration)).ToList();

    public IReadOnlyList<ISendable<Mute>> OnErrorCreate { get; }
  }
}
