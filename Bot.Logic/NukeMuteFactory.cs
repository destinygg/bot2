using System.Collections.Generic;
using System.Linq;
using Bot.Logic.Interfaces;
using Bot.Models.Interfaces;
using Bot.Models.Sendable;
using Bot.Tools;
using Bot.Tools.Interfaces;

namespace Bot.Logic {
  public class NukeMuteFactory : NukeAegisBase, IErrorableFactory<IParsedNuke, IReadOnlyList<IReceived<IUser, ITransmittable>>, IReadOnlyList<ISendable<ITransmittable>>> {

    public IReadOnlyList<ISendable<ITransmittable>> Create(IParsedNuke nuke, IReadOnlyList<IReceived<IUser, ITransmittable>> context) =>
      GetCurrentVictims(nuke, context)
      .Select(u => new SendableMute(u, nuke.Duration)).ToList();

    public IReadOnlyList<ISendable<ITransmittable>> OnErrorCreate => new SendableError($"An error occured in {nameof(NukeMuteFactory)}.").Wrap().ToList();
  }
}
